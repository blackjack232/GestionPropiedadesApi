using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Request.APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;

namespace APLICACION_GESTION_PROPIEDADES.Servicios
{
	public class PropiedadServicio : IPropiedadAplicacion
	{

		private readonly IPropiedadRespositorio _propiedadRepositorio;
		private readonly IOwnerRepositorio _ownerRepositorio;
		private readonly IPropertyImageRepositorio _imageRepositorio;
		private readonly ICloudinaryServicio _cloudinaryServicio;
		private readonly ILogger<PropiedadServicio> _logger;

		public PropiedadServicio(IPropiedadRespositorio propiedadRepositorio, IOwnerRepositorio ownerRepositorio, IPropertyImageRepositorio imageRepositorio, ILogger<PropiedadServicio> logger)
		{
			_propiedadRepositorio = propiedadRepositorio;
			_ownerRepositorio = ownerRepositorio;
			_imageRepositorio = imageRepositorio;
			_logger = logger;
		}


		/// <summary>
		/// Obtiene una lista de propiedades filtradas por nombre, dirección o rango de precios,
		/// incluyendo todas sus imágenes asociadas.
		/// </summary>
		/// <param name="name">Nombre parcial o completo de la propiedad (opcional).</param>
		/// <param name="address">Dirección parcial o completa de la propiedad (opcional).</param>
		/// <param name="minPrice">Precio mínimo de la propiedad (opcional).</param>
		/// <param name="maxPrice">Precio máximo de la propiedad (opcional).</param>
		/// <returns>
		/// ApiResponse con código 200 y lista de propiedades si tiene éxito.
		/// Código 500 si ocurre un error interno.
		/// </returns>
		public async Task<ApiResponse<IEnumerable<PropertyDto>>> ObtenerPropiedad(string? name, string? address, decimal? minPrice, decimal? maxPrice)
		{
			try
			{
				_logger.LogInformation(Constantes.ConsultarPropiedades, name, address, minPrice, maxPrice);


				var properties = await _propiedadRepositorio.ObtenerPropiedad(name, address, minPrice, maxPrice);

				var resultados = new List<PropertyDto>();

				foreach (var prop in properties)
				{
					var imagenes = await _imageRepositorio.ObtenerImagenesPorPropiedad(prop.IdProperty);

					resultados.Add(new PropertyDto
					{
						IdProperty = prop.IdProperty,
						IdOwner = prop.IdOwner,
						Name = prop.Name,
						Address = prop.Address,
						Price = prop.Price,
						ImageUrls = imagenes.Select(i => i.File).ToList()
					});
				}

				_logger.LogInformation(Constantes.PropiedadesObtenidas, resultados.Count);
				return ApiResponse<IEnumerable<PropertyDto>>.Ok(resultados, Constantes.PropiedadesObtenidasExito);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorObtenerPropiedades, name, address, minPrice, maxPrice);
				return ApiResponse<IEnumerable<PropertyDto>>.Fail(Constantes.ErrorObtenerPropiedadesMensaje);
			}
		}

		/// <summary>
		/// Obtiene los detalles de una propiedad por su ID, incluyendo sus imágenes asociadas.
		/// </summary>
		/// <param name="id">ID de la propiedad.</param>
		/// <returns>Objeto PropertyDto con información detallada y sus imágenes, dentro de un ApiResponse.</returns>
		public async Task<ApiResponse<PropertyDto?>> ObtenerPorId(string id)
		{
			try
			{
				_logger.LogInformation(Constantes.ConsultarPropiedadPorId, id);

				var property = await _propiedadRepositorio.ObtenerPorId(id);

				if (property == null)
				{
					_logger.LogWarning(Constantes.PropiedadNoEncontrada, id);
					return ApiResponse<PropertyDto?>.Fail(Constantes.PropiedadNoExisteMensaje);
				}

				var imagenes = await _imageRepositorio.ObtenerImagenesPorPropiedad(property.IdProperty);

				var dto = new PropertyDto
				{
					IdOwner = property.IdOwner,
					Name = property.Name,
					Address = property.Address,
					Price = property.Price,
					ImageUrls = imagenes.Select(i => i.File).ToList()
				};

				return ApiResponse<PropertyDto?>.Ok(dto, Constantes.PropiedadObtenidaMensaje);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorObtenerPropiedadPorId, id);
				return ApiResponse<PropertyDto?>.Fail(Constantes.ErrorConsultarPropiedadMensaje);
			}
		}

		/// <summary>
		/// Crea una nueva propiedad si el IdOwner proporcionado existe.
		/// </summary>
		/// <param name="dto">DTO con los datos de la propiedad.</param>
		/// <returns>
		/// ApiResponse con código 200 si fue creada correctamente,
		/// 400 si el IdOwner no existe,
		/// o 500 si ocurre un error inesperado.
		/// </returns>
		public async Task<ApiResponse<string>> Crear(PropertyRequest dto)
		{
			try
			{
				_logger.LogInformation(Constantes.IntentandoCrearPropiedad, dto.IdOwner);

				if (!await _ownerRepositorio.ExisteOwner(dto.IdOwner))
				{
					_logger.LogWarning(Constantes.IdOwnerNoExiste, dto.IdOwner);
					return ApiResponse<string>.Fail(string.Format(Constantes.IdOwnerNoExisteMensaje, dto.IdOwner));
				}

				var property = new Property
				{
					IdOwner = dto.IdOwner,
					Name = dto.Name,
					Address = dto.Address,
					Price = dto.Price,
					CodeInternal = dto.CodeInternal,
					Year = dto.Year
				};

				await _propiedadRepositorio.Crear(property);
				_logger.LogInformation(Constantes.PropiedadCreada, dto.IdOwner);


				return ApiResponse<string>.Ok(null, Constantes.PropiedadCreadaMensaje);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorCrearPropiedad, dto.IdOwner);
				return ApiResponse<string>.Fail(Constantes.ErrorCrearPropiedadMensaje);
			}
		}
		/// <summary>
		/// Actualiza los datos de una propiedad existente por su ID.
		/// </summary>
		/// <param name="id">ID de la propiedad a actualizar.</param>
		/// <param name="dto">DTO con los nuevos datos de la propiedad.</param>
		/// <returns>
		/// ApiResponse con mensaje de éxito si la propiedad fue actualizada.<br/>
		/// Retorna un error si la propiedad no existe o si ocurre una excepción durante la operación.
		/// </returns>

		public async Task<ApiResponse<string>> Actualizar(string id, PropertyRequest dto)
		{
			try
			{
				var existe = await _propiedadRepositorio.ObtenerPorId(id);
				if (existe == null)
					return ApiResponse<string>.Fail(Constantes.PropiedadNoExisteMensaje);

				var propiedad = new Property
				{
					IdProperty = id,
					IdOwner = dto.IdOwner,
					Name = dto.Name,
					Address = dto.Address,
					Price = dto.Price,
					CodeInternal = dto.CodeInternal,
					Year = dto.Year
				};

				var actualizado = await _propiedadRepositorio.Actualizar(id, propiedad);

				if (!actualizado)
					return ApiResponse<string>.Fail(Constantes.ErrorActualizarPropiedadMensaje);

				_logger.LogInformation(Constantes.PropiedadActualizada, id);
				return ApiResponse<string>.Ok(null, Constantes.PropiedadActualizadaMensaje);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorActualizarPropiedad, id);
				return ApiResponse<string>.Fail(Constantes.ErrorActualizarPropiedadMensaje);
			}
		}

		public async Task<ApiResponse<string>> Eliminar(string id)
		{
			try
			{
				var eliminado = await _propiedadRepositorio.Eliminar(id);
				if (!eliminado)
					return ApiResponse<string>.Fail(Constantes.PropiedadNoExisteMensaje);

				_logger.LogInformation(Constantes.PropiedadEliminada, id);
				return ApiResponse<string>.Ok(null, Constantes.PropiedadEliminadaMensaje);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorEliminarPropiedad, id);
				return ApiResponse<string>.Fail(Constantes.ErrorEliminarPropiedadMensaje);
			}
		}

		/// <summary>
		/// Registra un propietario, una propiedad y una imagen de forma transaccional.
		/// </summary>
		/// <param name="request">Datos del registro completo.</param>
		/// <returns>ApiResponse con mensaje y estado del registro.</returns>
		public async Task<ApiResponse<string>> RegistrarPropiedadCompleta(PropiedadCompletaRequest request)
		{
			string? ownerId = null;
			string? propiedadId = null;

			try
			{
				var urlImagenproietario = await SubirImagen(request.Propietario.Photo, Constantes.ImagenPropietario);
				var ownerRequest = ConstruirEntidadPropietario(request.Propietario, urlImagenproietario);
				ownerId = await CrearOwner(ownerRequest);
				if (string.IsNullOrEmpty(ownerId))
					return ApiResponse<string>.Fail(Constantes.ErrorCrearOwner);

				propiedadId = await CrearPropiedad(request, ownerId);
				if (string.IsNullOrEmpty(propiedadId))
				{
					await _ownerRepositorio.Eliminar(ownerId);
					return ApiResponse<string>.Fail(Constantes.ErrorCrearPropiedad);
				}

				var urlImagenPropiedad = await SubirImagen(request.Imagen, Constantes.ImagenesPropiedades);
				if (string.IsNullOrEmpty(urlImagenPropiedad))
				{
					await Rollback(ownerId, propiedadId);
					return ApiResponse<string>.Fail(Constantes.ErrorSubirImagen);
				}

				await GuardarImagen(propiedadId, urlImagenPropiedad);

				return ApiResponse<string>.Ok(propiedadId, Constantes.MensajeRegistroExitoso);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.MensajeErrorRegistro);
				await Rollback(ownerId!, propiedadId!);
				return ApiResponse<string>.Fail(Constantes.MensajeErrorRegistro + ex.Message);
			}
		}

		/// <summary>
		/// Crea un nuevo propietario en la base de datos a partir de los datos enviados en la solicitud completa.
		/// </summary>
		/// <param name="request">Objeto con toda la información del registro, incluyendo los datos del propietario.</param>
		/// <returns>Identificador único del propietario creado.</returns>
		private async Task<string> CrearOwner(OwnerDto request)
		{
			return await _ownerRepositorio.Crear(request);
		}

		/// <summary>
		/// Crea una nueva propiedad y la asocia al propietario especificado.
		/// </summary>
		/// <param name="request">Objeto con la información completa de la solicitud, incluyendo los datos de la propiedad.</param>
		/// <param name="ownerId">Identificador único del propietario al cual se vinculará la propiedad.</param>
		/// <returns>Identificador único de la propiedad creada.</returns>
		private async Task<string> CrearPropiedad(PropiedadCompletaRequest request, string ownerId)
		{
			var propiedad = ConstruirEntidadPropiedad(request.Propiedad, ownerId);
			return await _propiedadRepositorio.Crear(propiedad);
		}

		/// <summary>
		/// Sube un archivo de imagen a Cloudinary y retorna la URL pública generada.
		/// </summary>
		/// <param name="imagen">Archivo de imagen en formato IFormFile.</param>
		/// <param name="file">Nombre con el que se guardará el archivo en Cloudinary.</param>
		/// <returns>URL pública de la imagen almacenada en Cloudinary.</returns>
		private async Task<string> SubirImagen(IFormFile imagen, string file)
		{
			return await _cloudinaryServicio.SubirImagen(imagen, file);
		}

		/// <summary>
		/// Guarda en la base de datos el registro de una imagen asociada a una propiedad.
		/// </summary>
		/// <param name="propiedadId">Identificador de la propiedad a la que pertenece la imagen.</param>
		/// <param name="url">URL de la imagen almacenada en la nube.</param>
		private async Task GuardarImagen(string propiedadId, string url)
		{
			await _imageRepositorio.Crear(new PropertyImage
			{
				IdProperty = propiedadId,
				File = url,
				Enabled = true
			});
		}

		/// <summary>
		/// Revierte la creación de un propietario y/o propiedad eliminando los registros creados previamente.
		/// </summary>
		/// <param name="ownerId">Identificador del propietario a eliminar.</param>
		/// <param name="propiedadId">Identificador de la propiedad a eliminar.</param>
		private async Task Rollback(string ownerId, string propiedadId)
		{
			if (!string.IsNullOrEmpty(propiedadId))
				await _propiedadRepositorio.Eliminar(propiedadId);
			if (!string.IsNullOrEmpty(ownerId))
				await _ownerRepositorio.Eliminar(ownerId);
		}

		/// <summary>
		/// Construye un objeto <see cref="Property"/> a partir de un DTO y el ID del propietario.
		/// </summary>
		/// <param name="dto">Datos de la propiedad en formato <see cref="PropertyRequest"/>.</param>
		/// <param name="ownerId">Identificador único del propietario asociado.</param>
		/// <returns>Entidad <see cref="Property"/> lista para ser almacenada en base de datos.</returns>
		private static Property ConstruirEntidadPropiedad(PropertyRequest dto, string ownerId)
		{
			return new Property
			{
				IdOwner = ownerId,
				Name = dto.Name,
				Address = dto.Address,
				Price = dto.Price,
				CodeInternal = dto.CodeInternal,
				Year = dto.Year
			};
		}


		private static OwnerDto ConstruirEntidadPropietario(OwnerRequest owner, string url)
		{
			return new OwnerDto
			{
				Name = owner.Name,
				Address = owner.Address,
				Photo = url,
				Birthday = owner.Birthday
			};
		}

	}

}

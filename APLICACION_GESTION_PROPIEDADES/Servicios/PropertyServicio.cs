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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace APLICACION_GESTION_PROPIEDADES.Servicios
{
	public class PropertyServicio : IPropertyAplicacion
	{

		private readonly IPropertyRespositorio _propiedadRepositorio;
		private readonly IOwnerRepositorio _ownerRepositorio;
		private readonly IPropertyImageRepositorio _imageRepositorio;
		private readonly ICloudinaryServicio _cloudinaryServicio;
		private readonly ILogger<PropertyServicio> _logger;

		public PropertyServicio(IPropertyRespositorio propiedadRepositorio, IOwnerRepositorio ownerRepositorio, IPropertyImageRepositorio imageRepositorio, ILogger<PropertyServicio> logger, ICloudinaryServicio cloudinaryServicio)
		{
			_propiedadRepositorio = propiedadRepositorio;
			_ownerRepositorio = ownerRepositorio;
			_imageRepositorio = imageRepositorio;
			_cloudinaryServicio = cloudinaryServicio;
			_logger = logger;
		}


		/// <summary>
		/// Obtiene una lista paginada de propiedades filtradas por nombre, dirección o rango de precios,
		/// incluyendo todas sus imágenes asociadas.
		/// </summary>
		/// <param name="name">Nombre parcial o completo de la propiedad (opcional).</param>
		/// <param name="address">Dirección parcial o completa de la propiedad (opcional).</param>
		/// <param name="minPrice">Precio mínimo de la propiedad (opcional).</param>
		/// <param name="maxPrice">Precio máximo de la propiedad (opcional).</param>
		/// <param name="pageNumber">Número de página (default: 1).</param>
		/// <param name="pageSize">Tamaño de página (default: 10, max: 100).</param>
		/// <returns>
		/// ApiResponse con código 200 y lista paginada de propiedades si tiene éxito.
		/// Código 500 si ocurre un error interno.
		/// </returns>
		public async Task<ApiResponse<PagedResponse<PropertyDto>>> ObtenerPropiedad(string? name, string? address, decimal? minPrice, decimal? maxPrice, int pageNumber = 1, int pageSize = 10)
		{
			try
			{
				// Validar parámetros de paginación
				pageSize = Math.Min(pageSize, 100); // Limitar máximo 100 registros por página
				pageNumber = Math.Max(pageNumber, 1); // Asegurar que sea al menos 1

				_logger.LogInformation(MessageResponse.ConsultarPropiedades, name, address, minPrice, maxPrice, pageNumber, pageSize);

				// Obtener propiedades paginadas
				var (properties, totalCount) = await _propiedadRepositorio.ObtenerPropiedadPaginada(
					name, address, minPrice, maxPrice, pageNumber, pageSize);

				// Optimización: Obtener todas las imágenes en una sola consulta
				var propertyIds = properties.Select(p => p.IdProperty).ToList();
				var allImages = await _imageRepositorio.ObtenerImagenesPorPropiedades(propertyIds);
				var imagesDictionary = allImages.GroupBy(i => i.IdProperty)
											  .ToDictionary(g => g.Key, g => g.Select(i => i.File).ToList());

				var resultados = properties.Select(prop => new PropertyDto
				{
					IdProperty = prop.IdProperty,
					IdOwner = prop.IdOwner,
					Name = prop.Name,
					Address = prop.Address,
					Price = prop.Price,
					Year = prop.Year,
					CodeInternal = prop.CodeInternal,
					ImageUrls = imagesDictionary.TryGetValue(prop.IdProperty, out var urls) ? urls : new List<string>()
				}).ToList();

				// Construir respuesta paginada
				var pagedResponse = new PagedResponse<PropertyDto>
				{
					PageNumber = pageNumber,
					PageSize = pageSize,
					TotalRecords = totalCount,
					TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
					Data = resultados
				};

				_logger.LogInformation(MessageResponse.PropiedadesObtenidas, resultados.Count);
				return ApiResponse<PagedResponse<PropertyDto>>.Ok(pagedResponse, MessageResponse.PropiedadesObtenidasExito);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorObtenerPropiedades, name, address, minPrice, maxPrice);
				return ApiResponse<PagedResponse<PropertyDto>>.Fail(MessageResponse.ErrorObtenerPropiedadesMensaje);
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
				_logger.LogInformation(MessageResponse.ConsultarPropiedadPorId, id);

				var property = await _propiedadRepositorio.ObtenerPorId(id);

				if (property == null)
				{
					_logger.LogWarning(MessageResponse.PropiedadNoEncontrada, id);
					return ApiResponse<PropertyDto?>.Fail(MessageResponse.PropiedadNoExisteMensaje);
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

				return ApiResponse<PropertyDto?>.Ok(dto, MessageResponse.PropiedadObtenidaMensaje);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorObtenerPropiedadPorId, id);
				return ApiResponse<PropertyDto?>.Fail(MessageResponse.ErrorConsultarPropiedadMensaje);
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
				_logger.LogInformation(MessageResponse.IntentandoCrearPropiedad, dto.IdOwner);

				if (!await _ownerRepositorio.ExisteOwner(dto.IdOwner))
				{
					_logger.LogWarning(MessageResponse.IdOwnerNoExiste, dto.IdOwner);
					return ApiResponse<string>.Fail(string.Format(MessageResponse.IdOwnerNoExisteMensaje, dto.IdOwner));
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
				_logger.LogInformation(MessageResponse.PropiedadCreada, dto.IdOwner);


				return ApiResponse<string>.Ok(null, MessageResponse.PropiedadCreadaMensaje);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorCrearPropiedad, dto.IdOwner);
				return ApiResponse<string>.Fail(MessageResponse.ErrorCrearPropiedadMensaje);
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
					return ApiResponse<string>.Fail(MessageResponse.PropiedadNoExisteMensaje);

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
					return ApiResponse<string>.Fail(MessageResponse.ErrorActualizarPropiedadMensaje);

				_logger.LogInformation(MessageResponse.PropiedadActualizada, id);
				return ApiResponse<string>.Ok(null, MessageResponse.PropiedadActualizadaMensaje);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorActualizarPropiedad, id);
				return ApiResponse<string>.Fail(MessageResponse.ErrorActualizarPropiedadMensaje);
			}
		}

		public async Task<ApiResponse<string>> Eliminar(string id)
		{
			try
			{
				var eliminado = await _propiedadRepositorio.Eliminar(id);
				if (!eliminado)
					return ApiResponse<string>.Fail(MessageResponse.PropiedadNoExisteMensaje);

				_logger.LogInformation(MessageResponse.PropiedadEliminada, id);
				return ApiResponse<string>.Ok(null, MessageResponse.PropiedadEliminadaMensaje);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorEliminarPropiedad, id);
				return ApiResponse<string>.Fail(MessageResponse.ErrorEliminarPropiedadMensaje);
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
				var urlImagenproietario = await SubirImagen(request.Propietario.Photo, MessageResponse.ImagenPropietario);
				var ownerRequest = ConstruirEntidadPropietario(request.Propietario, urlImagenproietario);
				ownerId = await CrearOwner(ownerRequest);
				if (string.IsNullOrEmpty(ownerId))
					return ApiResponse<string>.Fail(MessageResponse.ErrorCrearOwner);

				propiedadId = await CrearPropiedad(request, ownerId);
				if (string.IsNullOrEmpty(propiedadId))
				{
					await _ownerRepositorio.Eliminar(ownerId);
					return ApiResponse<string>.Fail(MessageResponse.ErrorCrearPropiedad);
				}

				var urlImagenPropiedad = await SubirImagen(request.Imagen, MessageResponse.ImagenesPropiedades);
				if (string.IsNullOrEmpty(urlImagenPropiedad))
				{
					await Rollback(ownerId, propiedadId);
					return ApiResponse<string>.Fail(MessageResponse.ErrorSubirImagen);
				}

				await GuardarImagen(propiedadId, urlImagenPropiedad);

				return ApiResponse<string>.Ok(propiedadId, MessageResponse.MensajeRegistroExitoso);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.MensajeErrorRegistro);
				await Rollback(ownerId!, propiedadId!);
				return ApiResponse<string>.Fail(MessageResponse.MensajeErrorRegistro + ex.Message);
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

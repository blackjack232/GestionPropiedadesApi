using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using Microsoft.Extensions.Logging;

namespace APLICACION_GESTION_PROPIEDADES.Servicios
{
	public class PropertyImageServicio : IPropertyImageAplicacion
	{
		private readonly IPropertyImageRepositorio _imageRepositorio;
		private readonly IPropertyRespositorio _propiedadRepositorio;
		private readonly ILogger<PropertyImageServicio> _logger;

		public PropertyImageServicio(IPropertyImageRepositorio imageRepositorio,
									 IPropertyRespositorio propiedadRepositorio,
									 ILogger<PropertyImageServicio> logger)
		{
			_imageRepositorio = imageRepositorio;
			_propiedadRepositorio = propiedadRepositorio;
			_logger = logger;
		}

		public async Task<ApiResponse<string>> Crear(PropertyImageRequest request)
		{
			try
			{
				var propiedad = await _propiedadRepositorio.ObtenerPorId(request.IdProperty);
				if (propiedad == null)
				{
					_logger.LogWarning(MessageResponse.PropiedadNoEncontrada, request.IdProperty);
					return ApiResponse<string>.Fail(MessageResponse.PropiedadNoExisteMensaje);
				}

				var nuevaImagen = new PropertyImage
				{
					IdProperty = request.IdProperty,
					File = request.File,
					Enabled = request.Enabled,

				};

				await _imageRepositorio.Crear(nuevaImagen);
				_logger.LogInformation(MessageResponse.ImagenCreada, request);
				return ApiResponse<string>.Ok(null, MessageResponse.ImagenCreadaCorrectamente);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorCrearImagen);
				return ApiResponse<string>.Fail(MessageResponse.ErrorCrearImagen);
			}
		}

		public async Task<ApiResponse<string>> Eliminar(string id)
		{
			try
			{
				var eliminado = await _imageRepositorio.Eliminar(id);
				if (!eliminado)
					return ApiResponse<string>.Fail(MessageResponse.ImagenNoEncontradaEliminar);

				_logger.LogInformation(MessageResponse.ImagenEliminada, id);
				return ApiResponse<string>.Ok(null, MessageResponse.ImagenEliminadaCorrectamente);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorEliminarImagen);
				return ApiResponse<string>.Fail(MessageResponse.ErrorEliminarImagen);
			}
		}

		public async Task<ApiResponse<IEnumerable<PropertyImageDto>>> ObtenerPorIdPropiedad(string idProperty)
		{
			try
			{
				var imagenes = await _imageRepositorio.ObtenerPorIdPropiedad(idProperty);
				var listaDto = imagenes.Select(i => new PropertyImageDto
				{
					IdPropertyImage = i.IdPropertyImage,
					IdProperty = i.IdProperty,
					File = i.File,
					Enabled = i.Enabled
				});

				return ApiResponse<IEnumerable<PropertyImageDto>>.Ok(listaDto, MessageResponse.ImagenesObtenidasCorrectamente);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorObtenerImagenes);
				return ApiResponse<IEnumerable<PropertyImageDto>>.Fail(MessageResponse.ErrorObtenerImagenes);
			}
		}

	}
}

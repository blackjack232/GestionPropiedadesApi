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


	namespace APLICACION_GESTION_PROPIEDADES.Servicios
	{
		public class PropertyTraceServicio : IPropertyTraceAplicacion
		{
			private readonly IPropertyTraceRepositorio _traceRepositorio;
			private readonly IPropertyRespositorio _propiedadRepositorio;
			private readonly ILogger<PropertyTraceServicio> _logger;

			public PropertyTraceServicio(IPropertyTraceRepositorio traceRepositorio,
										 IPropertyRespositorio propiedadRepositorio,
										 ILogger<PropertyTraceServicio> logger)
			{
				_traceRepositorio = traceRepositorio;
				_propiedadRepositorio = propiedadRepositorio;
				_logger = logger;
			}

			public async Task<ApiResponse<string>> Crear(PropertyTraceRequest request)
			{
				try
				{
					var propiedad = await _propiedadRepositorio.ObtenerPorId(request.PropertyId);
					if (propiedad == null)
					{
						_logger.LogWarning(MessageResponse.PropiedadNoEncontrada, request.PropertyId);
						return ApiResponse<string>.Fail(MessageResponse.PropiedadNoExisteMensaje);
					}

					var trace = new PropertyTrace
					{
						IdProperty = request.PropertyId,
						DateSale = request.DateSale,
						Name = request.Name,
						Value = request.Value,
						Tax = request.Tax
					};

					await _traceRepositorio.Crear(trace);

					_logger.LogInformation("Traza de propiedad creada exitosamente");
					return ApiResponse<string>.Ok(null, MessageResponse.TrazaCreadaMensaje);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, MessageResponse.ErrorCrearTraza);
					return ApiResponse<string>.Fail(MessageResponse.ErrorCrearTraza);
				}
			}

			public async Task<ApiResponse<string>> Eliminar(string id)
			{
				try
				{
					var eliminado = await _traceRepositorio.Eliminar(id);
					if (!eliminado)
					{
						_logger.LogWarning("No se encontró la traza con id {id} para eliminar", id);
						return ApiResponse<string>.Fail(MessageResponse.TrazaNoExisteMensaje);
					}

					_logger.LogInformation("Traza de propiedad eliminada: {Id}", id);
					return ApiResponse<string>.Ok(null, MessageResponse.TrazaEliminadaMensaje);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, MessageResponse.ErrorEliminarTraza);
					return ApiResponse<string>.Fail(MessageResponse.ErrorEliminarTraza);
				}
			}

			public async Task<ApiResponse<IEnumerable<PropertyTraceDto>>> ObtenerPorIdPropiedad(string idProperty)
			{
				try
				{
					var trazas = await _traceRepositorio.ObtenerPorIdPropiedad(idProperty);
					if (trazas == null || !trazas.Any())
					{
						_logger.LogWarning("No se encontraron trazas para la propiedad {id}", idProperty);
						return ApiResponse<IEnumerable<PropertyTraceDto>>.Fail(MessageResponse.TrazaNoExisteMensaje);
					}

					var dtoList = trazas.Select(t => new PropertyTraceDto
					{
						IdPropertyTrace = t.IdPropertyTrace,
						IdProperty = t.IdProperty,
						DateSale = t.DateSale,
						Name = t.Name,
						Value = t.Value,
						Tax = t.Tax
					});

					return ApiResponse<IEnumerable<PropertyTraceDto>>.Ok(dtoList, MessageResponse.TrazaObtenidaMensaje);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, MessageResponse.ErrorObtenerTraza);
					return ApiResponse<IEnumerable<PropertyTraceDto>>.Fail(MessageResponse.ErrorObtenerTraza);
				}
			}
		}
	}

}

using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using Microsoft.AspNetCore.Mvc;

namespace API_GESTION_PROPIEDADES.Controllers.PropertyTrace
{
	[Route("api/[controller]")]
	[ApiController]
	public class PropertyTraceController : ControllerBase
	{
		private readonly IPropertyTraceAplicacion _propertyTraceAplicacion;
		private readonly ILogger<PropertyTraceController> _logger;

		public PropertyTraceController(IPropertyTraceAplicacion propertyTraceAplicacion, ILogger<PropertyTraceController> logger)
		{
			_propertyTraceAplicacion = propertyTraceAplicacion;
			_logger = logger;
		}

		[HttpGet("{idProperty}")]
		[ProducesResponseType(typeof(ApiResponse<IEnumerable<PropertyTraceDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ObtenerPorIdPropiedad(string idProperty)
		{
			try
			{
				var response = await _propertyTraceAplicacion.ObtenerPorIdPropiedad(idProperty);

				if (!response.Success || response.Data == null)
					return NotFound(ApiResponse<object>.Fail(MessageResponse.TrazoNoExisteMensaje));

				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorObtenerTrazoPorPropiedad, idProperty);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Crear([FromBody] PropertyTraceRequest request)
		{
			if (!ModelState.IsValid)
			{
				var errores = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();

				return BadRequest(ApiResponse<List<string>>.Fail(string.Join("; ", errores)));
			}

			try
			{
				var response = await _propertyTraceAplicacion.Crear(request);

				if (!response.Success)
					return BadRequest(response);

				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorCrearTrazoPropiedad);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Eliminar(string id)
		{
			try
			{
				var response = await _propertyTraceAplicacion.Eliminar(id);

				if (!response.Success)
					return BadRequest(response);

				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorEliminarTrazoPropiedad, id);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}
	}
}

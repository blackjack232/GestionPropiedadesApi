using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Request.APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace API_GESTION_PROPIEDADES.Controllers.Propiedad
{
	[Route("api/[controller]")]
	[ApiController]
	public class PropertyController : ControllerBase
	{
		private readonly IPropertyAplicacion _propiedadAplicacion;
		private readonly ILogger<PropertyController> _logger;

		public PropertyController(IPropertyAplicacion propiedadAplicacion, ILogger<PropertyController> logger)
		{
			_propiedadAplicacion = propiedadAplicacion;
			_logger = logger;
		}

		[HttpGet]
		[ProducesResponseType(typeof(ApiResponse<IEnumerable<PropertyDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ObtenerPropiedad([FromQuery] string? name, [FromQuery] string? address, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			try
			{
				var response = await _propiedadAplicacion.ObtenerPropiedad(
					name,
					address,
					minPrice,
					maxPrice,
					pageNumber,
					pageSize);

				if (!response.Success)
					return BadRequest(response);

				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorObtenerPropiedad);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ApiResponse<PropertyDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ObtenerPorId(string id)
		{
			try
			{
				var response = await _propiedadAplicacion.ObtenerPorId(id);

				if (!response.Success || response.Data == null)
					return NotFound(ApiResponse<object>.Fail(MessageResponse.PropiedadNoExisteMensaje));

				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorObtenerPorId, id);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Crear([FromBody] PropertyRequest property)
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
				var response = await _propiedadAplicacion.Crear(property);

				if (!response.Success)
					return BadRequest(response);

				return Ok(ApiResponse<string>.Ok(response.Data!, MessageResponse.CreacionExitosa));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorCrear);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Eliminar(string id)
		{
			try
			{
				var response = await _propiedadAplicacion.Eliminar(id);

				if (!response.Success)
				{
					if (response.Message == MessageResponse.PropiedadNoExisteMensaje || response.Message.Contains("no existe"))
						return NotFound(ApiResponse<object>.Fail(MessageResponse.PropiedadNoExisteMensaje));

					return BadRequest(response);
				}

				return Ok(ApiResponse<string>.Ok(id, MessageResponse.EliminacionExitosa));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorEliminar, id);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpPut("{id}")]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Actualizar(string id, [FromBody] PropertyRequest property)
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
				var response = await _propiedadAplicacion.Actualizar(id, property);

				if (!response.Success)
				{
					if (response.Message == MessageResponse.PropiedadNoExisteMensaje || response.Message.Contains("no existe"))
						return NotFound(ApiResponse<object>.Fail(MessageResponse.PropiedadNoExisteMensaje));

					return BadRequest(response);
				}

				return Ok(ApiResponse<string>.Ok(id, MessageResponse.ActualizacionExitosa));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorActualizar, id);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpPost("registro-completo")]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> RegistrarPropiedadCompleta([FromForm] PropiedadCompletaRequest request)
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
				var response = await _propiedadAplicacion.RegistrarPropiedadCompleta(request);

				if (!response.Success)
					return BadRequest(response);

				return Ok(ApiResponse<string>.Ok(response.Data!, MessageResponse.RegistroCompletoExitoso));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorRegistrarPropiedadCompleta);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}
	}
}

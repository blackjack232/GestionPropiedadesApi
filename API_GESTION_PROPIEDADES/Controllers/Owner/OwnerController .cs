using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using Microsoft.AspNetCore.Mvc;

namespace API_GESTION_PROPIEDADES.Controllers.Owner
{
	[ApiController]
	[Route("api/[controller]")]
	public class OwnerController : ControllerBase
	{
		private readonly IOwnerAplicacion _servicio;
		private readonly ILogger<OwnerController> _logger;

		public OwnerController(IOwnerAplicacion servicio, ILogger<OwnerController> logger)
		{
			_servicio = servicio;
			_logger = logger;
		}

		[HttpGet]
		[ProducesResponseType(typeof(ApiResponse<IEnumerable<OwnerResponse>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ObtenerTodos()
		{
			try
			{
				var response = await _servicio.ObtenerTodos();

				if (!response.Success)
					return BadRequest(response);

				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorObtenerTodosPropietarios);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ApiResponse<OwnerResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ObtenerPorId(string id)
		{
			try
			{
				var response = await _servicio.ObtenerPorId(id);

				if (!response.Success || response.Data == null)
					return NotFound(ApiResponse<object>.Fail(MessageResponse.PropietarioNoExisteMensaje));

				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorObtenerPropietarioPorId, id);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Crear([FromBody] OwnerDto owner)
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
				var response = await _servicio.Crear(owner);

				if (!response.Success)
					return BadRequest(response);

				return Ok(ApiResponse<string>.Ok(response.Data!, MessageResponse.CreacionExitosa));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorCrearPropietario);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpPut("{id}")]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Actualizar(string id, [FromBody] OwnerDto ownerDto)
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
				var response = await _servicio.Actualizar(id, ownerDto);

				if (!response.Success)
				{
					if (response.Message == MessageResponse.PropietarioNoExisteMensaje || response.Message.Contains("no existe"))
						return NotFound(ApiResponse<object>.Fail(MessageResponse.PropietarioNoExisteMensaje));

					return BadRequest(response);
				}

				return Ok(ApiResponse<string>.Ok(id, MessageResponse.ActualizacionExitosa));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorActualizarPropietario, id);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Eliminar(string id)
		{
			try
			{
				var response = await _servicio.Eliminar(id);

				if (!response.Success)
					return NotFound(ApiResponse<object>.Fail(MessageResponse.PropietarioNoExisteMensaje));

				return Ok(ApiResponse<string>.Ok(id, MessageResponse.EliminacionExitosa));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.LogErrorEliminarPropietario, id);
				var errorResponse = ApiResponse<object>.Fail(MessageResponse.ErrorInternoServidor);
				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}
	}
}

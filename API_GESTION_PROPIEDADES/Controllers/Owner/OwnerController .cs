using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using Microsoft.AspNetCore.Mvc;

namespace API_GESTION_PROPIEDADES.Controllers.Owner
{
	[ApiController]
	[Route("api/[controller]")]
	public class OwnerController : ControllerBase
	{
		private readonly IOwnerAplicacion _servicio;

		public OwnerController(IOwnerAplicacion servicio)
		{
			_servicio = servicio;
		}

		/// <summary>
		/// Obtiene la lista de todos los propietarios.
		/// </summary>
		/// <returns>
		/// Código 200 con la lista de propietarios.<br/>
		/// Código 400 si ocurre un error inesperado.
		/// </returns>
		[HttpGet]
		[ProducesResponseType(typeof(ApiResponse<IEnumerable<OwnerResponse>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> ObtenerTodos()
		{
			var response = await _servicio.ObtenerTodos();
			if (!response.Success)
				return BadRequest(response);

			return Ok(response);
		}

		/// <summary>
		/// Obtiene un propietario por su ID.
		/// </summary>
		/// <param name="id">ID del propietario a consultar.</param>
		/// <returns>
		/// Código 200 con el propietario si existe.<br/>
		/// Código 404 si no se encuentra el propietario.
		/// </returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ApiResponse<OwnerResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> ObtenerPorId(string id)
		{
			var response = await _servicio.ObtenerPorId(id);
			if (!response.Success)
				return NotFound(response);

			return Ok(response);
		}

		/// <summary>
		/// Crea un nuevo propietario.
		/// </summary>
		/// <param name="owner">Datos del propietario a crear.</param>
		/// <returns>
		/// Código 200 si se crea correctamente.<br/>
		/// Código 400 si los datos no son válidos.
		/// </returns>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Crear([FromBody] OwnerRequest owner)
		{
			if (!ModelState.IsValid)
			{
				var errores = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();

				return BadRequest(errores);
			}
			var response = await _servicio.Crear(owner);
			if (!response.Success)
				return BadRequest(response);

			return Ok(response);
		}


		/// <summary>
		/// Actualiza un propietario existente.
		/// </summary>
		/// <param name="id">ID del propietario a actualizar.</param>
		/// <param name="ownerDto">Datos del propietario.</param>
		/// <returns>
		/// Código 200 si se actualizó correctamente.<br/>
		/// Código 404 si no se encontró.<br/>
		/// Código 400 si los datos no son válidos.
		/// </returns>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Actualizar(string id, [FromBody] OwnerRequest ownerDto)
		{
			if (!ModelState.IsValid)
			{
				var errores = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();

				return BadRequest(errores);
			}
			var response = await _servicio.Actualizar(id, ownerDto);

			if (!response.Success)
			{
				if (response.Message == Constantes.PropiedadNoExisteMensaje || response.Message.Contains("no existe"))
					return NotFound(response);

				return BadRequest(response);
			}

			return Ok(response);
		}

		/// <summary>
		/// Elimina un propietario por su ID.
		/// </summary>
		/// <param name="id">ID del propietario.</param>
		/// <returns>
		/// Código 200 si fue eliminado correctamente.<br/>
		/// Código 404 si no se encuentra el propietario.
		/// </returns>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Eliminar(string id)
		{
			var response = await _servicio.Eliminar(id);

			if (!response.Success)
				return NotFound(response);

			return Ok(response);
		}

	}

}

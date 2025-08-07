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

		public PropertyTraceController(IPropertyTraceAplicacion propertyTraceAplicacion)
		{
			_propertyTraceAplicacion = propertyTraceAplicacion;
		}

		/// <summary>
		/// Obtiene los trazos de una propiedad por su Id.
		/// </summary>
		[HttpGet("{idProperty}")]
		[ProducesResponseType(typeof(ApiResponse<IEnumerable<PropertyTraceDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> ObtenerPorIdPropiedad(string idProperty)
		{
			var response = await _propertyTraceAplicacion.ObtenerPorIdPropiedad(idProperty);

			if (!response.Success || response.Data == null)
				return NotFound(response);

			return Ok(response);
		}

		/// <summary>
		/// Crea un trazo de propiedad (historial de venta).
		/// </summary>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Crear([FromBody] PropertyTraceRequest request)
		{
			if (!ModelState.IsValid)
			{
				var errores = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();

				return BadRequest(new ApiResponse<object>
				{
					Success = false,
					Message = "Errores de validación",
					Data = errores
				});
			}

			var response = await _propertyTraceAplicacion.Crear(request);

			if (!response.Success)
				return BadRequest(response);

			return Ok(response);
		}

		/// <summary>
		/// Elimina un trazo por su Id.
		/// </summary>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Eliminar(string id)
		{
			var response = await _propertyTraceAplicacion.Eliminar(id);

			if (!response.Success)
				return BadRequest(response);

			return Ok(response);
		}
	}
}

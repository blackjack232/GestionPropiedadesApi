using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using Microsoft.AspNetCore.Mvc;

namespace API_GESTION_PROPIEDADES.Controllers.PropertyImage
{
	[Route("api/[controller]")]
	[ApiController]
	public class PropertyImageController : ControllerBase
	{
		private readonly IPropertyImageAplicacion _propertyImageAplicacion;

		public PropertyImageController(IPropertyImageAplicacion propertyImageAplicacion)
		{
			_propertyImageAplicacion = propertyImageAplicacion;
		}

		/// <summary>
		/// Obtiene las imágenes de una propiedad por su Id.
		/// </summary>
		[HttpGet("{idProperty}")]
		[ProducesResponseType(typeof(ApiResponse<IEnumerable<PropertyImageDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ObtenerPorIdPropiedad(string idProperty)
		{
			var response = await _propertyImageAplicacion.ObtenerPorIdPropiedad(idProperty);

			if (!response.Success || response.Data == null)
				return NotFound(response);

			return Ok(response);
		}

		/// <summary>
		/// Crea una imagen para una propiedad existente.
		/// </summary>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Crear([FromBody] PropertyImageRequest request)
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

			var response = await _propertyImageAplicacion.Crear(request);

			if (!response.Success)
				return BadRequest(response);

			return Ok(response);
		}

		/// <summary>
		/// Elimina una imagen por su Id.
		/// </summary>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Eliminar(string id)
		{
			var response = await _propertyImageAplicacion.Eliminar(id);

			if (!response.Success)
				return BadRequest(response);

			return Ok(response);
		}
	}
}

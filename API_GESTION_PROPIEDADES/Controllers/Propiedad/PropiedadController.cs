using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace API_GESTION_PROPIEDADES.Controllers.Propiedad
{
	[Route("api/[controller]")]
	[ApiController]
	public class PropiedadController : ControllerBase
	{
		private readonly IPropiedadAplicacion _propiedadAplicacion;

		public PropiedadController(IPropiedadAplicacion propiedadAplicacion)
		{
			_propiedadAplicacion = propiedadAplicacion;
		}


		/// <summary>
		/// Obtiene una lista de propiedades con filtros opcionales por nombre, dirección y rango de precios.
		/// </summary>
		/// <param name="name">Nombre parcial o completo de la propiedad (opcional).</param>
		/// <param name="address">Dirección parcial o completa de la propiedad (opcional).</param>
		/// <param name="minPrice">Precio mínimo (opcional).</param>
		/// <param name="maxPrice">Precio máximo (opcional).</param>
		/// <returns>
		/// Código 200 con la lista de propiedades si la operación es exitosa.<br/>
		/// Código 400 si ocurre un error de lógica o validación.
		/// </returns>
		[HttpGet]
		[ProducesResponseType(typeof(ApiResponse<IEnumerable<PropertyDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> ObtenerPropiedad([FromQuery] string? name, [FromQuery] string? address, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
		{
			var response = await _propiedadAplicacion.ObtenerPropiedad(name, address, minPrice, maxPrice);

			if (!response.Success)
				return BadRequest(response);

			return Ok(response);
		}

		/// <summary>
		/// Obtiene los detalles de una propiedad por su ID.
		/// </summary>
		/// <param name="id">ID de la propiedad a consultar.</param>
		/// <returns>
		/// Código 200 con los detalles de la propiedad si existe.<br/>
		/// Código 404 si no se encuentra la propiedad.<br/>
		/// Código 400 si ocurre un error de validación.
		/// </returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ApiResponse<PropertyDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> ObtenerPorId(string id)
		{
			var response = await _propiedadAplicacion.ObtenerPorId(id);

			if (!response.Success || response.Data == null)
				return NotFound(response);

			return Ok(response);
		}

		/// <summary>
		/// Crea una nueva propiedad si el propietario (IdOwner) existe.
		/// </summary>
		/// <param name="property">Objeto PropertyDto con los datos de la nueva propiedad.</param>
		/// <returns>
		/// Código 200 si la propiedad se crea correctamente.<br/>
		/// Código 400 si el IdOwner no existe o si hay errores de validación.
		/// </returns>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Crear([FromBody] PropertyDto property)
		{
			var response = await _propiedadAplicacion.Crear(property);

			if (!response.Success)
				return BadRequest(response);

			return Ok(response);
		}

	}
}

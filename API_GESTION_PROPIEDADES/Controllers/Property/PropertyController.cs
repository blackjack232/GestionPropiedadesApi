using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
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


		public PropertyController(IPropertyAplicacion propiedadAplicacion)
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
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		[HttpGet]
		[ProducesResponseType(typeof(ApiResponse<PagedResponse<PropertyDto>>), StatusCodes.Status200OK)]
		public async Task<IActionResult> ObtenerPropiedad([FromQuery] string? name,	[FromQuery] string? address,[FromQuery] decimal? minPrice,[FromQuery] decimal? maxPrice,[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)   
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
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
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
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Crear([FromBody] PropertyRequest property)
		{
			if (!ModelState.IsValid)
			{
				var errores = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();

				return BadRequest(errores);
			}
			var response = await _propiedadAplicacion.Crear(property);

			if (!response.Success)
				return BadRequest(response);

			return Ok(response);
		}
		/// <summary>
		/// Elimina una propiedad por su ID.
		/// </summary>
		/// <param name="id">ID de la propiedad a eliminar.</param>
		/// <returns>
		/// Código 200 si se elimina correctamente.<br/>
		/// Código 404 si no se encuentra la propiedad.<br/>
		/// Código 400 si ocurre un error de validación.
		/// </returns>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Eliminar(string id)
		{
			var response = await _propiedadAplicacion.Eliminar(id);

			if (!response.Success)
			{
				if (response.Message == Constantes.PropiedadNoExisteMensaje || response.Message.Contains("no existe"))
					return NotFound(response);

				return BadRequest(response);
			}

			return Ok(response);
		}

		/// <summary>
		/// Actualiza una propiedad existente.
		/// </summary>
		/// <param name="id">ID de la propiedad a actualizar.</param>
		/// <param name="property">Datos de la propiedad.</param>
		/// <returns>
		/// Código 200 si se actualiza correctamente.<br/>
		/// Código 404 si no se encuentra la propiedad.<br/>
		/// Código 400 si los datos no son válidos.
		/// </returns>
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

				return BadRequest(errores);
			}

			var response = await _propiedadAplicacion.Actualizar(id, property);

			if (!response.Success)
			{
				if (response.Message == Constantes.PropiedadNoExisteMensaje || response.Message.Contains("no existe"))
					return NotFound(response);

				return BadRequest(response);
			}

			return Ok(response);
		}

		/// <summary>
		/// Crea un registro completo incluyendo Owner, Property y PropertyImage. El proceso es transaccional.
		/// </summary>
		/// <param name="request">Datos completos para registrar propietario, propiedad e imagen.</param>
		/// <returns>
		/// Código 200 si se registra todo correctamente.<br/>
		/// Código 400 si ocurre un error en cualquier paso del proceso.<br/>
		/// Código 500 si ocurre una excepción inesperada.
		/// </returns>
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

				return BadRequest(errores);
			}
			var response = await _propiedadAplicacion.RegistrarPropiedadCompleta(request);

			if (!response.Success)
				return BadRequest(response);

			return Ok(response);


		}

	}
}

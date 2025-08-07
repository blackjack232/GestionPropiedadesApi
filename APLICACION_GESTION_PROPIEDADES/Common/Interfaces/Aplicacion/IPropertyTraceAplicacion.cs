using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;

namespace APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion
{
	public interface IPropertyTraceAplicacion
	{
		/// <summary>
		/// Crea una nueva traza de propiedad.
		/// </summary>
		/// <param name="request">Datos de la traza.</param>
		/// <returns>Respuesta con el ID creado o error.</returns>
		Task<ApiResponse<string>> Crear(PropertyTraceRequest request);

		/// <summary>
		/// Elimina una traza por su ID.
		/// </summary>
		/// <param name="id">ID de la traza.</param>
		/// <returns>Resultado de la operación.</returns>
		Task<ApiResponse<string>> Eliminar(string id);

		/// <summary>
		/// Obtiene todas las trazas de una propiedad específica.
		/// </summary>
		/// <param name="idProperty">ID de la propiedad.</param>
		/// <returns>Lista de trazas.</returns>
		Task<ApiResponse<IEnumerable<PropertyTraceDto>>> ObtenerPorIdPropiedad(string idProperty);
	}
}

using DOMINIO_GESTION_PROPIEDADES.Entities;

namespace APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio
{
	public interface IPropertyTraceRepositorio
	{
		/// <summary>
		/// Crea una nueva traza de propiedad.
		/// </summary>
		Task Crear(PropertyTrace propertyTrace);

		/// <summary>
		/// Elimina una traza por su ID.
		/// </summary>
		/// <param name="id">ID de la traza.</param>
		/// <returns>True si fue eliminada, false si no existe.</returns>
		Task<bool> Eliminar(string id);

		/// <summary>
		/// Obtiene todas las trazas de una propiedad por su ID.
		/// </summary>
		/// <param name="idProperty">ID de la propiedad.</param>
		Task<IEnumerable<PropertyTrace>> ObtenerPorIdPropiedad(string idProperty);
	}
}

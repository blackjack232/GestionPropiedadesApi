
using DOMINIO_GESTION_PROPIEDADES.Entities;

namespace APLICACION_GESTION_PROPIEDADES.Interfaces.Repositorio
{
	public interface IPropiedadRespositorio
	{

		Task<IEnumerable<Property>> ObtenerPropiedad(string? name, string? address, decimal? minPrice, decimal? maxPrice);
		Task<Property?> ObtenerPorId(string id);
		Task<string> Crear(Property property);
		Task<bool> Actualizar(string id, Property propiedad);
		Task<bool> Eliminar(string id);


	}
}

using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using DOMINIO_GESTION_PROPIEDADES.Entities;

namespace APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio
{
	public interface IOwnerRepositorio
	{
		Task<bool> ExisteOwner(string idOwner);
		Task<Owner?> ObtenerPorId(string id);
		Task Crear(OwnerRequest owner);
		Task<IEnumerable<Owner>> ObtenerTodos();
		Task<bool> Actualizar(string id, OwnerRequest owner);
		Task<bool> Eliminar(string id);
	}
}


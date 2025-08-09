using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using DOMINIO_GESTION_PROPIEDADES.Entities;

namespace APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio
{
	public interface IOwnerRepositorio
	{
		Task<bool> ExisteOwner(string idOwner);
		Task<Owner?> ObtenerPorId(string id);
		Task<string> Crear(OwnerDto owner);
		Task<IEnumerable<Owner>> ObtenerTodos();
		Task<bool> Actualizar(string id, OwnerDto owner);
		Task<bool> Eliminar(string id);
	}
}


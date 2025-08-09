using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using DOMINIO_GESTION_PROPIEDADES.Entities;

namespace APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion
{
	public interface IOwnerAplicacion
	{
		Task<ApiResponse<IEnumerable<OwnerResponse>>> ObtenerTodos();
		Task<ApiResponse<Owner?>> ObtenerPorId(string id);
		Task<ApiResponse<string>> Crear(OwnerDto owner);
		Task<ApiResponse<string>> Actualizar(string id, OwnerDto ownerDto);
		Task<ApiResponse<string>> Eliminar(string id);


	}
}

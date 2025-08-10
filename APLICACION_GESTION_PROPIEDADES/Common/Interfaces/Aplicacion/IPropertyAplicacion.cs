using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Request.APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;

namespace APLICACION_GESTION_PROPIEDADES.Interfaces.Aplicacion
{
	public interface IPropertyAplicacion
	{
		Task<ApiResponse<PagedResponse<PropertyDto>>> ObtenerPropiedad(string? name, string? address, decimal? minPrice, decimal? maxPrice, int pageNumber = 1, int pageSize = 10);
		Task<ApiResponse<PropertyDto?>> ObtenerPorId(string id);
		Task<ApiResponse<string>> Crear(PropertyRequest dto);
		Task<ApiResponse<string>> Actualizar(string id, PropertyRequest dto);
		Task<ApiResponse<string>> Eliminar(string id);
		Task<ApiResponse<string>> RegistrarPropiedadCompleta(PropiedadCompletaRequest registroPropiedadCompletaRequest);

	}
}

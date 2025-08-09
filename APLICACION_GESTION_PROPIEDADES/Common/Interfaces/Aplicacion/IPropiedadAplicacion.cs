using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Request.APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;

namespace APLICACION_GESTION_PROPIEDADES.Interfaces.Aplicacion
{
	public interface IPropiedadAplicacion
	{
		Task<ApiResponse<IEnumerable<PropertyDto>>> ObtenerPropiedad(string? name, string? address, decimal? minPrice, decimal? maxPrice);
		Task<ApiResponse<PropertyDto?>> ObtenerPorId(string id);
		Task<ApiResponse<string>> Crear(PropertyRequest dto);
		Task<ApiResponse<string>> Actualizar(string id, PropertyRequest dto);
		Task<ApiResponse<string>> Eliminar(string id);
		Task<ApiResponse<string>> RegistrarPropiedadCompleta(PropiedadCompletaRequest registroPropiedadCompletaRequest);

	}
}

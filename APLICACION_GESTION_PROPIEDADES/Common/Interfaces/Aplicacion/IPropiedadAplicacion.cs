using APLICACION_GESTION_PROPIEDADES.Dto;

namespace APLICACION_GESTION_PROPIEDADES.Interfaces.Aplicacion
{
	public interface IPropiedadAplicacion
	{
		Task<ApiResponse<IEnumerable<PropertyDto>>> ObtenerPropiedad(string? name, string? address, decimal? minPrice, decimal? maxPrice);
		Task<ApiResponse<PropertyDto?>> ObtenerPorId(string id);
		Task<ApiResponse<string>> Crear(PropertyDto dto);
	}
}

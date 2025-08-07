using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using Microsoft.AspNetCore.Mvc;

namespace APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion
{
	public interface IPropertyImageAplicacion
	{
		Task<ApiResponse<IEnumerable<PropertyImageDto>>> ObtenerPorIdPropiedad(string idProperty);
		Task<ApiResponse<string>> Crear([FromBody] PropertyImageRequest request);
		Task<ApiResponse<string>> Eliminar(string id);
	}
}

using Microsoft.AspNetCore.Http;

namespace APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion
{
	public interface ICloudinaryServicio
	{
		Task<string?> SubirImagen(IFormFile request, string file);
	}
}

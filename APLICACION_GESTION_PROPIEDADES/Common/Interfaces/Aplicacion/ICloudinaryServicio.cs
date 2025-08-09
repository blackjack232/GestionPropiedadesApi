using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion
{
	public interface ICloudinaryServicio
	{
		Task<string?> SubirImagen(IFormFile request , string file);
	}
}

using APLICACION_GESTION_PROPIEDADES.Dto.Request.APLICACION_GESTION_PROPIEDADES.Dto.Request;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLICACION_GESTION_PROPIEDADES.Dto.Request
{
	public class PropiedadCompletaRequest
	{
		/// <summary>
		/// Datos del propietario (owner).
		/// </summary>
		public OwnerRequest Propietario { get; set; } = null!;

		/// <summary>
		/// Datos de la propiedad.
		/// </summary>
		public PropertyRequest Propiedad { get; set; } = null!;

		/// <summary>
		/// Imagen de la propiedad (archivo).
		/// </summary>
		public IFormFile Imagen { get; set; } = null!;
	}
}

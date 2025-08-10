using System.ComponentModel.DataAnnotations;

namespace APLICACION_GESTION_PROPIEDADES.Dto.Request
{
	public class PropertyImageRequest
	{
		[Required]
		public string IdProperty { get; set; }

		[Required]
		public string File { get; set; }

		public bool Enabled { get; set; }
	}

}

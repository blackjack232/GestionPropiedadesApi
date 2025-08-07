using System.ComponentModel.DataAnnotations;


namespace APLICACION_GESTION_PROPIEDADES.Dto.Request
{
	/// <summary>
	/// Representa los datos requeridos para crear o actualizar un propietario.
	/// </summary>
	public class OwnerRequest
	{
		/// <summary>
		/// Nombre completo del propietario.
		/// </summary>
		[Required(ErrorMessage = "El nombre es obligatorio.")]
		[MaxLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
		public string Name { get; set; }

		/// <summary>
		/// Dirección del propietario.
		/// </summary>
		[Required(ErrorMessage = "La dirección es obligatoria.")]
		[MaxLength(200, ErrorMessage = "La dirección no puede tener más de 200 caracteres.")]
		public string Address { get; set; }

		/// <summary>
		/// Fecha de nacimiento del propietario.
		/// </summary>
		[Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
		public DateTime Birthday { get; set; }

		/// <summary>
		/// URL o ruta de la foto del propietario.
		/// </summary>
		[Required(ErrorMessage = "La foto es obligatoria.")]
		[MaxLength(300, ErrorMessage = "La foto no puede tener más de 300 caracteres.")]
		public string Photo { get; set; }
	}
}


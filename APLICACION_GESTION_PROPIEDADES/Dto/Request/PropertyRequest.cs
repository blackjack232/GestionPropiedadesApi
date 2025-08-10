namespace APLICACION_GESTION_PROPIEDADES.Dto.Request
{
	using System.ComponentModel.DataAnnotations;

	namespace APLICACION_GESTION_PROPIEDADES.Dto.Request
	{
		public class PropertyRequest
		{
			/// <summary>
			/// Identificador del propietario. Debe existir en el sistema.
			/// </summary>
			public string? IdOwner { get; set; } = null!;

			/// <summary>
			/// Nombre de la propiedad.
			/// </summary>
			[Required(ErrorMessage = "El campo Name es obligatorio.")]
			public string Name { get; set; } = null!;

			/// <summary>
			/// Dirección de la propiedad.
			/// </summary>
			[Required(ErrorMessage = "El campo Address es obligatorio.")]
			public string Address { get; set; } = null!;

			/// <summary>
			/// Precio de la propiedad.
			/// </summary>
			[Required(ErrorMessage = "El campo Price es obligatorio.")]
			[Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a cero.")]
			public decimal Price { get; set; }

			/// <summary>
			/// Código interno de la propiedad.
			/// </summary>
			[Required(ErrorMessage = "El campo CodeInternal es obligatorio.")]
			public string CodeInternal { get; set; } = null!;

			/// <summary>
			/// Año de construcción o publicación de la propiedad.
			/// </summary>
			[Required(ErrorMessage = "El campo Year es obligatorio.")]
			[Range(1800, 2100, ErrorMessage = "El año debe estar entre 1800 y 2100.")]
			public int Year { get; set; }

			/// <summary>
			/// ID único de la propiedad (opcional al crear, requerido al actualizar).
			/// </summary>
			public string? Id { get; set; }
		}
	}

}

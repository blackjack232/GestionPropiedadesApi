namespace APLICACION_GESTION_PROPIEDADES.Dto.Response
{
	public class OwnerResponse
	{
		/// <summary>
		/// Identificador del propietario.
		/// </summary>
		public string Id { get; set; } = null!;

		/// <summary>
		/// Nombre completo del propietario.
		/// </summary>
		public string Name { get; set; } = null!;

		/// <summary>
		/// Dirección del propietario.
		/// </summary>
		public string Address { get; set; } = null!;

		/// <summary>
		/// Fecha de nacimiento del propietario.
		/// </summary>
		public DateTime Birthday { get; set; }

		/// <summary>
		/// URL o nombre del archivo de la foto del propietario.
		/// </summary>
		public string Photo { get; set; } = null!;
	}
}

namespace APLICACION_GESTION_PROPIEDADES.Dto
{
	public class OwnerDto
	{

		public string Name { get; set; }

		/// <summary>
		/// Dirección del propietario.
		/// </summary>

		public string Address { get; set; }

		/// <summary>
		/// Fecha de nacimiento del propietario.
		/// </summary>

		public DateTime Birthday { get; set; }

		/// <summary>
		/// Imagen del propietario (archivo).
		/// </summary>

		public string Photo { get; set; }
	}
}

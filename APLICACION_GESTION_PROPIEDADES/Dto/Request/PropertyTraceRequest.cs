namespace APLICACION_GESTION_PROPIEDADES.Dto.Request
{
	public class PropertyTraceRequest
	{
		/// <summary>
		/// Valor de la propiedad en el momento de la transacción
		/// </summary>
		public decimal Value { get; set; }

		/// <summary>
		/// Impuesto aplicado sobre la propiedad
		/// </summary>
		public decimal Tax { get; set; }

		/// <summary>
		/// Fecha en la que se realizó el traspaso
		/// </summary>
		public DateTime DateSale { get; set; }

		/// <summary>
		/// Nombre del propietario en el momento del traspaso
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Identificador de la propiedad relacionada (clave foránea)
		/// </summary>
		public string PropertyId { get; set; }
	}

}

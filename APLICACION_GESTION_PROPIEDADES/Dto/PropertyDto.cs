namespace APLICACION_GESTION_PROPIEDADES.Dto
{
	public class PropertyDto
	{
		public string IdProperty { get; set; }
		public string IdOwner { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public decimal Price { get; set; }
		public string? CodeInternal { get; set; }
		public int Year { get; set; }
		public List<string> ImageUrls { get; set; } = new();
	}

}


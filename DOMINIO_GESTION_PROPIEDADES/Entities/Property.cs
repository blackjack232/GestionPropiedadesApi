using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DOMINIO_GESTION_PROPIEDADES.Entities
{
	public class Property
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string IdProperty { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public string CodeInternal { get; set; } = string.Empty;
		public int Year { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string IdOwner { get; set; } = string.Empty;
	}

}

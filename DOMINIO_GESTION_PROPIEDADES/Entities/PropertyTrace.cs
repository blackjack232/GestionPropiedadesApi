using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DOMINIO_GESTION_PROPIEDADES.Entities
{
	public class PropertyTrace
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string IdPropertyTrace { get; set; } = string.Empty;

		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime DateSale { get; set; }

		public string Name { get; set; } = string.Empty;
		public decimal Value { get; set; }
		public decimal Tax { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string IdProperty { get; set; } = string.Empty;
	}

}

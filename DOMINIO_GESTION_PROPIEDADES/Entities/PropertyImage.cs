using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DOMINIO_GESTION_PROPIEDADES.Entities
{
	public class PropertyImage
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string IdPropertyImage { get; set; } = string.Empty;

		[BsonRepresentation(BsonType.ObjectId)]
		public string IdProperty { get; set; } = string.Empty;

		public string File { get; set; } = string.Empty;
		public bool Enabled { get; set; }
	}

}

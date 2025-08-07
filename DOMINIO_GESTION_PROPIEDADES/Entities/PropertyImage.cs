using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

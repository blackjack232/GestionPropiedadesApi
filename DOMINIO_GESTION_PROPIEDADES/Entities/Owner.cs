using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMINIO_GESTION_PROPIEDADES.Entities
{

	public class Owner
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId Id { get; set; }

		public string Name { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string Photo { get; set; } = string.Empty;

		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime Birthday { get; set; }
	}

}

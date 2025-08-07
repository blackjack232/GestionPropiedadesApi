using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using INFRAESTRUCTURA_GESTION_PROPIEDADES.Contexto;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRAESTRUCTURA_GESTION_PROPIEDADES.Repositorio
{
	public class OwnerRepositorio : IOwnerRepositorio
	{
		private readonly IMongoCollection<Owner> _collection;

		public OwnerRepositorio(MongoDbContext context)
		{
			_collection = context.Owners;
		}

		public async Task<bool> ExisteOwner(string idOwner)
		{
			if (!ObjectId.TryParse(idOwner, out var objectId))
				return false;

			var filter = Builders<Owner>.Filter.Eq( o => o.Id, ObjectId.Parse(idOwner));
			return await _collection.Find(filter).AnyAsync();
		}
	}

}

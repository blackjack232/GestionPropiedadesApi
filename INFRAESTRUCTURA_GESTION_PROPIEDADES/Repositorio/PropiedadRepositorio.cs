using APLICACION_GESTION_PROPIEDADES.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using INFRAESTRUCTURA_GESTION_PROPIEDADES.Contexto;
using MongoDB.Bson;
using MongoDB.Driver;

namespace INFRAESTRUCTURA_GESTION_PROPIEDADES.Repositorio
{
	public class PropiedadRepositorio : IPropiedadRespositorio
	{
		private readonly IMongoCollection<Property> _collection;

		public PropiedadRepositorio(MongoDbContext context)
		{
			_collection = context.Properties;
		}

		public async Task<IEnumerable<Property>> ObtenerPropiedad(string? name, string? address, decimal? minPrice, decimal? maxPrice)
		{
			var filter = Builders<Property>.Filter.Empty;

			if (!string.IsNullOrEmpty(name))
				filter &= Builders<Property>.Filter.Regex("Name", new BsonRegularExpression(name, "i"));

			if (!string.IsNullOrEmpty(address))
				filter &= Builders<Property>.Filter.Regex("Address", new BsonRegularExpression(address, "i"));

			if (minPrice.HasValue)
				filter &= Builders<Property>.Filter.Gte("Price", minPrice.Value);

			if (maxPrice.HasValue)
				filter &= Builders<Property>.Filter.Lte("Price", maxPrice.Value);

			return await _collection.Find(filter).ToListAsync();
		}

		public async Task<Property?> ObtenerPorId(string id)
		{
			var filter = Builders<Property>.Filter.Eq("_id", ObjectId.Parse(id));
			return await _collection.Find(filter).FirstOrDefaultAsync();
		}

		public async Task Crear(Property property)
		{
			await _collection.InsertOneAsync(property);
		}

	}

}

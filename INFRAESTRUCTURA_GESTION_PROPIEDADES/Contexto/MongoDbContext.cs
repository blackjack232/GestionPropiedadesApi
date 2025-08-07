using APLICACION_GESTION_PROPIEDADES.Dto;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace INFRAESTRUCTURA_GESTION_PROPIEDADES.Contexto
{
	public class MongoDbContext
	{
		private readonly IMongoDatabase _database;

		public MongoDbContext(IOptions<MongoDbSettings> settings)
		{
			var client = new MongoClient(settings.Value.ConnectionString);
			_database = client.GetDatabase(settings.Value.DatabaseName);
		}

		public IMongoCollection<Property> Properties =>
			_database.GetCollection<Property>("Property");
		public IMongoCollection<PropertyImage> PropertyImages =>
		   _database.GetCollection<PropertyImage>("PropertyImage");

		public IMongoCollection<PropertyTrace> PropertyTraces =>
			_database.GetCollection<PropertyTrace>("PropertyTrace");

		public IMongoCollection<Owner> Owners =>
			_database.GetCollection<Owner>("Owner");
	}

}

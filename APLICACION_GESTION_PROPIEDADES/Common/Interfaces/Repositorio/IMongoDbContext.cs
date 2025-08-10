using DOMINIO_GESTION_PROPIEDADES.Entities;
using MongoDB.Driver;

namespace APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio
{
	public interface IMongoDbContext
	{
		IMongoCollection<Property> Properties { get; }
		IMongoCollection<PropertyImage> PropertyImages { get; }
		IMongoCollection<PropertyTrace> PropertyTraces { get; }
		IMongoCollection<Owner> Owners { get; }
	}

}

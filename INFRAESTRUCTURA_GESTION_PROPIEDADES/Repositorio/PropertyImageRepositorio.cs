using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using INFRAESTRUCTURA_GESTION_PROPIEDADES.Contexto;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRAESTRUCTURA_GESTION_PROPIEDADES.Repositorio
{
	public class PropertyImageRepositorio : IPropertyImageRepositorio
	{
		private readonly IMongoCollection<PropertyImage> _collection;

		public PropertyImageRepositorio(MongoDbContext context)
		{
			_collection = context.PropertyImages;
		}

		public async Task<List<PropertyImage>> ObtenerImagenesPorPropiedad(string idProperty)
		{
			return await _collection
				.Find(img => img.IdProperty == idProperty && img.Enabled)
				.ToListAsync();
		}
	}

}

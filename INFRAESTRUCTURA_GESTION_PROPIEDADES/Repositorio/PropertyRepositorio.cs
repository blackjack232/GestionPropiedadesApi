using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace INFRAESTRUCTURA_GESTION_PROPIEDADES.Repositorio
{
	public class PropertyRepositorio : IPropertyRespositorio
	{
		private readonly IMongoCollection<Property> _collection;
		private readonly ILogger<PropertyRepositorio> _logger;

		public PropertyRepositorio(IMongoDbContext context, ILogger<PropertyRepositorio> logger)
		{
			_collection = context.Properties;
			_logger = logger;
		}

		/// <summary>
		/// Obtiene una lista de propiedades que cumplan con los filtros proporcionados.
		/// </summary>
		public async Task<IEnumerable<Property>> ObtenerPropiedad(string? name, string? address, decimal? minPrice, decimal? maxPrice)
		{
			try
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

				var result = await _collection.Find(filter).ToListAsync();
				_logger.LogInformation(MessageResponse.FiltroPropiedades, result.Count);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorFiltroPropiedades);
				throw;
			}
		}
		/// <summary>
		/// Obtiene una lista paginada de propiedades que cumplan con los filtros proporcionados.
		/// </summary>
		/// <returns>Tupla con (propiedades, totalCount)</returns>
		public async Task<(IEnumerable<Property> Properties, long TotalCount)> ObtenerPropiedadPaginada(string? name, string? address, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
		{
			try
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

				// Obtener conteo total (sin paginación)
				var totalCount = await _collection.CountDocumentsAsync(filter);

				// Obtener datos paginados
				var properties = await _collection.Find(filter)
					.Skip((pageNumber - 1) * pageSize)
					.Limit(pageSize)
					.ToListAsync();

				_logger.LogInformation(MessageResponse.FiltroPropiedades, properties.Count);
				return (properties, totalCount);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorFiltroPropiedades);
				throw;
			}
		}

		/// <summary>
		/// Obtiene una propiedad por su ID.
		/// </summary>
		public async Task<Property?> ObtenerPorId(string id)
		{
			try
			{
				if (!ObjectId.TryParse(id, out var objectId))
				{
					_logger.LogWarning(MessageResponse.PropiedadIdInvalido, id);
					return null;
				}

				var filter = Builders<Property>.Filter.Eq("_id", objectId);
				var propiedad = await _collection.Find(filter).FirstOrDefaultAsync();

				if (propiedad == null)
					_logger.LogWarning(MessageResponse.PropiedadNoEncontrada, id);
				else
					_logger.LogInformation(MessageResponse.PropiedadEncontrada, id);

				return propiedad;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorObtenerPropiedad, id);
				throw;
			}
		}

		/// <summary>
		/// Crea una nueva propiedad en la base de datos.
		/// </summary>
		public async Task<string> Crear(Property property)
		{
			try
			{
				await _collection.InsertOneAsync(property);

				_logger.LogInformation(MessageResponse.PropiedadInsertada, property.IdOwner);

				// Retornar el Id generado como string
				return property.IdProperty.ToString();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorInsertarPropiedad, property.IdOwner);
				throw;
			}
		}

		/// <summary>
		/// Actualiza una propiedad existente en la base de datos por su ID.
		/// </summary>
		/// <param name="id">ID de la propiedad a actualizar.</param>
		/// <param name="property">Entidad Property con los nuevos datos.</param>
		/// <returns>
		/// True si la propiedad fue actualizada correctamente.<br/>
		/// False si no se encontró o no se modificó.<br/>
		/// Lanza una excepción si ocurre un error durante la operación.
		/// </returns>

		public async Task<bool> Actualizar(string id, Property propiedad)
		{
			try
			{
				var filter = Builders<Property>.Filter.Eq(p => p.IdProperty, id);
				var result = await _collection.ReplaceOneAsync(filter, propiedad);
				return result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorActualizarPropiedad, id);
				throw;
			}
		}
		/// <summary>
		/// Elimina una propiedad de la base de datos por su ID.
		/// </summary>
		/// <param name="id">ID de la propiedad a eliminar.</param>
		/// <returns>
		/// True si la propiedad fue eliminada exitosamente.<br/>
		/// False si no se encontró.<br/>
		/// Lanza una excepción si ocurre un error durante la operación.
		/// </returns>

		public async Task<bool> Eliminar(string id)
		{
			try
			{
				var filter = Builders<Property>.Filter.Eq(p => p.IdProperty, id);
				var result = await _collection.DeleteOneAsync(filter);
				return result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorEliminarPropiedad, id);
				throw;
			}
		}


	}

}

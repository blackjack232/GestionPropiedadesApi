using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using INFRAESTRUCTURA_GESTION_PROPIEDADES.Contexto;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace INFRAESTRUCTURA_GESTION_PROPIEDADES.Repositorio
{
	public class PropertyTraceRepositorio : IPropertyTraceRepositorio
	{

		private readonly IMongoCollection<PropertyTrace> _collection;
		private readonly ILogger<PropertyTraceRepositorio> _logger;

		public PropertyTraceRepositorio(MongoDbContext context, ILogger<PropertyTraceRepositorio> logger)
		{
			_collection = context.PropertyTraces;
			_logger = logger;
		}

		/// <inheritdoc/>
		public async Task Crear(PropertyTrace propertyTrace)
		{
			try
			{
				await _collection.InsertOneAsync(propertyTrace);
				_logger.LogInformation(Constantes.TrazaCreadaLog, propertyTrace.IdProperty);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorCrearTrazaLog);
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task<bool> Eliminar(string id)
		{
			try
			{
				var resultado = await _collection.DeleteOneAsync(trace => trace.IdPropertyTrace == id);
				if (resultado.DeletedCount == 0)
				{
					_logger.LogWarning(Constantes.TrazaNoEncontradaEliminarLog, id);
					return false;
				}

				_logger.LogInformation(Constantes.TrazaEliminadaLog, id);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorEliminarTrazaLog);
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<PropertyTrace>> ObtenerPorIdPropiedad(string idProperty)
		{
			try
			{
				var trazas = await _collection
					.Find(trace => trace.IdProperty == idProperty)
					.ToListAsync();

				_logger.LogInformation(Constantes.TrazasEncontradasLog, trazas.Count, idProperty);
				return trazas;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorObtenerTrazasLog, idProperty);
				throw;
			}
		}
	}

}

using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace INFRAESTRUCTURA_GESTION_PROPIEDADES.Repositorio
{
	public class PropertyTraceRepositorio : IPropertyTraceRepositorio
	{

		private readonly IMongoCollection<PropertyTrace> _collection;
		private readonly ILogger<PropertyTraceRepositorio> _logger;

		public PropertyTraceRepositorio(IMongoDbContext context, ILogger<PropertyTraceRepositorio> logger)
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
				_logger.LogInformation(MessageResponse.TrazaCreadaLog, propertyTrace.IdProperty);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorCrearTrazaLog);
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
					_logger.LogWarning(MessageResponse.TrazaNoEncontradaEliminarLog, id);
					return false;
				}

				_logger.LogInformation(MessageResponse.TrazaEliminadaLog, id);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorEliminarTrazaLog);
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
					.SortByDescending(trace => trace.DateSale) 
					.ToListAsync();

				_logger.LogInformation(MessageResponse.TrazasEncontradasLog, trazas.Count, idProperty);
				return trazas;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorObtenerTrazasLog, idProperty);
				throw;
			}
		}

	}

}

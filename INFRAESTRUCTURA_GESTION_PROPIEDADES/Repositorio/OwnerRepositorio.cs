using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using INFRAESTRUCTURA_GESTION_PROPIEDADES.Contexto;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace INFRAESTRUCTURA_GESTION_PROPIEDADES.Repositorio
{
	public class OwnerRepositorio : IOwnerRepositorio
	{
		private readonly IMongoCollection<Owner> _collection;
		private readonly ILogger<OwnerRepositorio> _logger;

		public OwnerRepositorio(MongoDbContext context, ILogger<OwnerRepositorio> logger)
		{
			_collection = context.Owners;
			_logger = logger;
		}

		/// <summary>
		/// Verifica si existe un propietario (Owner) con el ID proporcionado.
		/// </summary>
		/// <param name="idOwner">ID del propietario.</param>
		/// <returns>True si existe, False si no existe o si el ID es inválido.</returns>
		public async Task<bool> ExisteOwner(string idOwner)
		{
			try
			{
				if (!ObjectId.TryParse(idOwner, out var objectId))
				{
					_logger.LogWarning(Constantes.IdOwnerInvalido, idOwner);
					return false;
				}

				var filter = Builders<Owner>.Filter.Eq(o => o.Id, objectId);
				var existe = await _collection.Find(filter).AnyAsync();

				_logger.LogInformation(Constantes.VerificacionExistenciaOwner, idOwner, existe);
				return existe;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorVerificarExistenciaOwner, idOwner);
				throw;
			}
		}
	}

}

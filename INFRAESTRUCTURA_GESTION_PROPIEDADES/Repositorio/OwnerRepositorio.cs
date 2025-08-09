using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
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

		/// <summary>
		/// Obtiene un propietario por su ID.
		/// </summary>
		public async Task<Owner?> ObtenerPorId(string id)
		{
			try
			{
				if (!ObjectId.TryParse(id, out var objectId))
				{
					_logger.LogWarning(Constantes.IdOwnerInvalido, id);
					return null;
				}

				var filter = Builders<Owner>.Filter.Eq(o => o.Id, objectId);
				var owner = await _collection.Find(filter).FirstOrDefaultAsync();

				_logger.LogInformation(Constantes.OwnerObtenidoPorId, id);
				return owner;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorObtenerOwnerPorId, id);
				throw;
			}
		}

		/// <summary>
		/// Obtiene todos los propietarios.
		/// </summary>
		public async Task<IEnumerable<Owner>> ObtenerTodos()
		{
			try
			{
				var owners = await _collection.Find(_ => true).ToListAsync();
				_logger.LogInformation(Constantes.OwnersObtenidos);
				return owners;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorObtenerTodosOwners);
				throw;
			}
		}

		/// <summary>
		/// Crea un nuevo propietario en la base de datos.
		/// </summary>
		/// <param name="owner">Datos del propietario en formato OwnerRequest.</param>
		/// <returns>Id del propietario creado.</returns>
		public async Task<string> Crear(OwnerDto owner)
		{
			try
			{
				var entidad = new Owner
				{
					Name = owner.Name,
					Address = owner.Address,
					Photo = owner.Photo,
					Birthday = owner.Birthday
				};

				await _collection.InsertOneAsync(entidad);

				_logger.LogInformation(Constantes.OwnerCreado, entidad.Name);

				// Retorna el Id generado por MongoDB
				return entidad.Id.ToString();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorCrearOwner, owner.Name);
				throw;
			}
		}


		/// <summary>
		/// Actualiza los datos de un propietario existente por su ID.
		/// </summary>
		/// <param name="id">ID del propietario a actualizar.</param>
		/// <param name="owner">Objeto Owner con los nuevos datos.</param>
		/// <returns>True si se actualizó, false si no se encontró o no se modificó.</returns>
		public async Task<bool> Actualizar(string id, OwnerDto owner)
		{
			try
			{
				if (!ObjectId.TryParse(id, out var objectId))
				{
					_logger.LogWarning(Constantes.IdOwnerInvalido, id);
					return false;
				}

				var filter = Builders<Owner>.Filter.Eq(o => o.Id, objectId);

				var entidad = new Owner
				{
					Id = objectId,
					Name = owner.Name,
					Address = owner.Address,
					Photo = owner.Photo,
					Birthday = owner.Birthday
				};

				var result = await _collection.ReplaceOneAsync(filter, entidad);

				if (result.ModifiedCount > 0)
				{
					_logger.LogInformation(Constantes.OwnerActualizado, id);
					return true;
				}

				_logger.LogWarning(Constantes.OwnerNoActualizado, id);
				return false;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorActualizarOwner, id);
				throw;
			}
		}


		/// <summary>
		/// Elimina un propietario por su ID.
		/// </summary>
		public async Task<bool> Eliminar(string id)
		{
			try
			{
				if (!ObjectId.TryParse(id, out var objectId))
				{
					_logger.LogWarning(Constantes.IdOwnerInvalido, id);
					return false;
				}

				var result = await _collection.DeleteOneAsync(o => o.Id == objectId);
				_logger.LogInformation(Constantes.OwnerEliminado, id);
				return result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorEliminarOwner, id);
				throw;
			}
		}

	
	}
}

using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace INFRAESTRUCTURA_GESTION_PROPIEDADES.Repositorio
{
	public class PropertyImageRepositorio : IPropertyImageRepositorio
	{
		private readonly IMongoCollection<PropertyImage> _collection;
		private readonly ILogger<PropertyImageRepositorio> _logger;

		public PropertyImageRepositorio(IMongoDbContext context, ILogger<PropertyImageRepositorio> logger)
		{
			_collection = context.PropertyImages;
			_logger = logger;
		}

		/// <summary>
		/// Obtiene todas las imágenes habilitadas asociadas a una propiedad por su ID.
		/// </summary>
		/// <param name="idProperty">ID de la propiedad.</param>
		/// <returns>Lista de imágenes asociadas a la propiedad.</returns>
		public async Task<List<PropertyImage>> ObtenerImagenesPorPropiedad(string idProperty)
		{
			try
			{
				_logger.LogInformation(MessageResponse.ConsultarImagenesPorPropiedad, idProperty);

				var result = await _collection
					.Find(img => img.IdProperty == idProperty && img.Enabled)
					.ToListAsync();

				_logger.LogInformation(MessageResponse.ImagenesEncontradas, result.Count, idProperty);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorConsultarImagenes, idProperty);
				throw;
			}
		}
		/// <summary>
		/// Crea una nueva imagen de propiedad.
		/// </summary>
		public async Task Crear(PropertyImage propertyImage)
		{
			try
			{
				await _collection.InsertOneAsync(propertyImage);
				_logger.LogInformation(MessageResponse.ImagenCreada, propertyImage);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorCrearImagen);
				throw;
			}
		}

		/// <summary>
		/// Elimina (lógicamente) una imagen por su ID.
		/// </summary>
		public async Task<bool> Eliminar(string id)
		{
			try
			{
				var filtro = Builders<PropertyImage>.Filter.Eq(p => p.IdPropertyImage, id);
				var actualizacion = Builders<PropertyImage>.Update.Set(p => p.Enabled, false);

				var resultado = await _collection.UpdateOneAsync(filtro, actualizacion);

				if (resultado.ModifiedCount == 0)
				{
					_logger.LogWarning(MessageResponse.ImagenNoEncontradaEliminar, id);
					return false;
				}

				_logger.LogInformation(MessageResponse.ImagenEliminada, id);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorEliminarImagen);
				throw;
			}
		}

		/// <summary>
		/// Obtiene todas las imágenes activas de una propiedad por su ID.
		/// </summary>
		public async Task<IEnumerable<PropertyImage>> ObtenerPorIdPropiedad(string idProperty)
		{
			try
			{
				var filtro = Builders<PropertyImage>.Filter.And(
					Builders<PropertyImage>.Filter.Eq(p => p.IdProperty, idProperty),
					Builders<PropertyImage>.Filter.Eq(p => p.Enabled, true)
				);

				var imagenes = await _collection.Find(filtro).ToListAsync();

				_logger.LogInformation(MessageResponse.ImagenesObtenidasCorrectamente, idProperty);
				return imagenes;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorObtenerImagenes);
				throw;
			}
		}
		/// <summary>
		/// Obtiene todas las imágenes habilitadas para un conjunto de propiedades en una sola consulta
		/// </summary>
		public async Task<IEnumerable<PropertyImage>> ObtenerImagenesPorPropiedades(IEnumerable<string> propertyIds)
		{
			var filter = Builders<PropertyImage>.Filter.In("IdProperty", propertyIds) &
						 Builders<PropertyImage>.Filter.Eq("Enabled", true);

			return await _collection.Find(filter).ToListAsync();
		}


	}
}

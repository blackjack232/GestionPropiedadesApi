using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using INFRAESTRUCTURA_GESTION_PROPIEDADES.Contexto;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace INFRAESTRUCTURA_GESTION_PROPIEDADES.Repositorio
{
	public class PropertyImageRepositorio : IPropertyImageRepositorio
	{
		private readonly IMongoCollection<PropertyImage> _collection;
		private readonly ILogger<PropertyImageRepositorio> _logger;

		public PropertyImageRepositorio(MongoDbContext context, ILogger<PropertyImageRepositorio> logger)
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
				_logger.LogInformation(Constantes.ConsultarImagenesPorPropiedad, idProperty);

				var result = await _collection
					.Find(img => img.IdProperty == idProperty && img.Enabled)
					.ToListAsync();

				_logger.LogInformation(Constantes.ImagenesEncontradas, result.Count, idProperty);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorConsultarImagenes, idProperty);
				throw;
			}
		}
	}

}

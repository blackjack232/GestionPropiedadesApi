using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using System;

namespace APLICACION_GESTION_PROPIEDADES.Servicios
{
	public class PropiedadServicio : IPropiedadAplicacion
	{

		private readonly IPropiedadRespositorio _propiedadRepositorio;
		private readonly IOwnerRepositorio _ownerRepositorio;
		private readonly IPropertyImageRepositorio _imageRepositorio;

		public PropiedadServicio(IPropiedadRespositorio propiedadRepositorio, IOwnerRepositorio ownerRepositorio, IPropertyImageRepositorio imageRepositorio)
		{
			_propiedadRepositorio = propiedadRepositorio;
			_ownerRepositorio = ownerRepositorio;
			_imageRepositorio = imageRepositorio;
		}

	

		/// <summary>
		/// Obtiene una lista de propiedades filtradas por nombre, dirección o rango de precios,
		/// incluyendo todas sus imágenes asociadas.
		/// </summary>
		/// <param name="name">Nombre parcial o completo de la propiedad (opcional).</param>
		/// <param name="address">Dirección parcial o completa de la propiedad (opcional).</param>
		/// <param name="minPrice">Precio mínimo de la propiedad (opcional).</param>
		/// <param name="maxPrice">Precio máximo de la propiedad (opcional).</param>
		/// <returns>Lista de propiedades con sus respectivas imágenes dentro de un ApiResponse.</returns>
		public async Task<ApiResponse<IEnumerable<PropertyDto>>> ObtenerPropiedad(string? name, string? address, decimal? minPrice, decimal? maxPrice)
		{
			var properties = await _propiedadRepositorio.ObtenerPropiedad(name, address, minPrice, maxPrice);

			var resultados = new List<PropertyDto>();

			foreach (var prop in properties)
			{
				var imagenes = await _imageRepositorio.ObtenerImagenesPorPropiedad(prop.IdProperty);

				resultados.Add(new PropertyDto
				{
					IdOwner = prop.IdOwner,
					Name = prop.Name,
					Address = prop.Address,
					Price = prop.Price,
					ImageUrls = imagenes.Select(i => i.File).ToList()
				});
			}

			return ApiResponse<IEnumerable<PropertyDto>>.Ok(resultados, "Propiedades obtenidas correctamente");
		}


		/// <summary>
		/// Obtiene los detalles de una propiedad por su ID, incluyendo sus imágenes asociadas.
		/// </summary>
		/// <param name="id">ID de la propiedad.</param>
		/// <returns>Objeto PropertyDto con información detallada y sus imágenes, dentro de un ApiResponse.</returns>
		public async Task<ApiResponse<PropertyDto?>> ObtenerPorId(string id)
		{
			var property = await _propiedadRepositorio.ObtenerPorId(id);

			if (property == null)
				return ApiResponse<PropertyDto?>.Fail("La propiedad no existe.");

			var imagenes = await _imageRepositorio.ObtenerImagenesPorPropiedad(property.IdProperty);

			var dto = new PropertyDto
			{
				IdOwner = property.IdOwner,
				Name = property.Name,
				Address = property.Address,
				Price = property.Price,
				ImageUrls = imagenes.Select(i => i.File).ToList()
			};

			return ApiResponse<PropertyDto?>.Ok(dto, "Propiedad obtenida correctamente");
		}


		public async Task<ApiResponse<string>> Crear(PropertyDto dto)
		{
			if (!await _ownerRepositorio.ExisteOwner(dto.IdOwner))
			{
				return ApiResponse<string>.Fail($"No se puede crear la propiedad. El IdOwner '{dto.IdOwner}' no existe.");
			}

			var property = new Property
			{
				IdOwner = dto.IdOwner,
				Name = dto.Name,
				Address = dto.Address,
				Price = dto.Price,
				CodeInternal = dto.CodeInternal,
				Year = dto.Year
			};

			await _propiedadRepositorio.Crear(property);
			return ApiResponse<string>.Ok(null, "Propiedad creada exitosamente");
		}


	
	}

}

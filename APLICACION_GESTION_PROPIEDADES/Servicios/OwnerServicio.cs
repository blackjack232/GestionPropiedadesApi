using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using Microsoft.Extensions.Logging;

namespace APLICACION_GESTION_PROPIEDADES.Servicios
{
	public class OwnerServicio : IOwnerAplicacion
	{
		private readonly IOwnerRepositorio _repo;
		private readonly ILogger<OwnerServicio> _logger;

		public OwnerServicio(IOwnerRepositorio repo, ILogger<OwnerServicio> logger)
		{
			_repo = repo;
			_logger = logger;
		}

		/// <summary>
		/// Obtiene todos los propietarios.
		/// </summary>
		public async Task<ApiResponse<IEnumerable<OwnerResponse>>> ObtenerTodos()
		{
			try
			{
				var owners = await _repo.ObtenerTodos();
				var response = owners.Select(o => new OwnerResponse
				{
					Id = o.Id.ToString(),
					Name = o.Name,
					Address = o.Address,
					Birthday = o.Birthday,
					Photo = o.Photo
				});

				return ApiResponse<IEnumerable<OwnerResponse>>.Ok(response, Constantes.PropietariosObtenidos);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorObtenerPropietarios);
				return ApiResponse<IEnumerable<OwnerResponse>>.Fail(Constantes.ErrorObtenerPropietarios);
			}
		}


		/// <summary>
		/// Obtiene un propietario por su ID.
		/// </summary>
		public async Task<ApiResponse<Owner?>> ObtenerPorId(string id)
		{
			try
			{
				var owner = await _repo.ObtenerPorId(id);
				if (owner == null)
					return ApiResponse<Owner?>.Fail(Constantes.PropietarioNoEncontrado);

				return ApiResponse<Owner?>.Ok(owner, Constantes.PropietarioObtenido);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorObtenerPropietario);
				return ApiResponse<Owner?>.Fail(Constantes.ErrorObtenerPropietario);
			}
		}

		/// <summary>
		/// Crea un nuevo propietario.
		/// </summary>
		public async Task<ApiResponse<string>> Crear(OwnerRequest owner)
		{
			try
			{
				await _repo.Crear(owner);
				return ApiResponse<string>.Ok(null, Constantes.PropietarioCreado);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorCrearPropietario);
				return ApiResponse<string>.Fail(Constantes.ErrorCrearPropietario);
			}
		}

		/// <summary>
		/// Actualiza un propietario existente por ID.
		/// </summary>
		public async Task<ApiResponse<string>> Actualizar(string id, OwnerRequest ownerDto)
		{
			try
			{
				var existente = await _repo.ObtenerPorId(id);
				if (existente == null)
					return ApiResponse<string>.Fail(Constantes.PropietarioNoEncontrado);

				await _repo.Actualizar(id, ownerDto);
				return ApiResponse<string>.Ok(null, Constantes.PropietarioActualizado);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorActualizarPropietario);
				return ApiResponse<string>.Fail(Constantes.ErrorActualizarPropietario);
			}
		}

		/// <summary>
		/// Elimina un propietario por ID.
		/// </summary>
		public async Task<ApiResponse<string>> Eliminar(string id)
		{
			try
			{
				var existente = await _repo.ObtenerPorId(id);
				if (existente == null)
					return ApiResponse<string>.Fail(Constantes.PropietarioNoEncontrado);

				await _repo.Eliminar(id);
				return ApiResponse<string>.Ok(null, Constantes.PropietarioEliminado);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, Constantes.ErrorEliminarPropietario);
				return ApiResponse<string>.Fail(Constantes.ErrorEliminarPropietario);
			}
		}
	}

}

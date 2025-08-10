using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
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

				return ApiResponse<IEnumerable<OwnerResponse>>.Ok(response, MessageResponse.PropietariosObtenidos);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorObtenerPropietarios);
				return ApiResponse<IEnumerable<OwnerResponse>>.Fail(MessageResponse.ErrorObtenerPropietarios);
			}
		}


		/// <summary>
		/// Obtiene un propietario por su ID.
		/// </summary>
		public async Task<ApiResponse<OwnerResponse?>> ObtenerPorId(string id)
		{
			try
			{
				var owner = await _repo.ObtenerPorId(id);
				var ownerResponse = new OwnerResponse
				{
					Id = owner.Id.ToString(),
					Name = owner.Name,
					Address = owner.Address,
					Birthday = owner.Birthday,
					Photo = owner.Photo
				};
				if (owner == null)
					return ApiResponse<OwnerResponse?>.Fail(MessageResponse.PropietarioNoEncontrado);

				return ApiResponse<OwnerResponse?>.Ok(ownerResponse, MessageResponse.PropietarioObtenido);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorObtenerPropietario);
				return ApiResponse<OwnerResponse?>.Fail(MessageResponse.ErrorObtenerPropietario);
			}
		}

		/// <summary>
		/// Crea un nuevo propietario.
		/// </summary>
		public async Task<ApiResponse<string>> Crear(OwnerDto owner)
		{
			try
			{
				await _repo.Crear(owner);
				return ApiResponse<string>.Ok(null, MessageResponse.PropietarioCreado);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorCrearPropietario);
				return ApiResponse<string>.Fail(MessageResponse.ErrorCrearPropietario);
			}
		}

		/// <summary>
		/// Actualiza un propietario existente por ID.
		/// </summary>
		public async Task<ApiResponse<string>> Actualizar(string id, OwnerDto ownerDto)
		{
			try
			{
				var existente = await _repo.ObtenerPorId(id);
				if (existente == null)
					return ApiResponse<string>.Fail(MessageResponse.PropietarioNoEncontrado);

				await _repo.Actualizar(id, ownerDto);
				return ApiResponse<string>.Ok(null, MessageResponse.PropietarioActualizado);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorActualizarPropietario);
				return ApiResponse<string>.Fail(MessageResponse.ErrorActualizarPropietario);
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
					return ApiResponse<string>.Fail(MessageResponse.PropietarioNoEncontrado);

				await _repo.Eliminar(id);
				return ApiResponse<string>.Ok(null, MessageResponse.PropietarioEliminado);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, MessageResponse.ErrorEliminarPropietario);
				return ApiResponse<string>.Fail(MessageResponse.ErrorEliminarPropietario);
			}
		}

	}

}

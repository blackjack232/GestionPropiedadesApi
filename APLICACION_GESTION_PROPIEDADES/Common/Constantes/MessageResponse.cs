namespace APLICACION_GESTION_PROPIEDADES.Common.Constantes
{

	public static class MessageResponse
	{

		// ========== LOGS PARA CONSULTAS ==========
		public const string ConsultarPropiedades = "Obteniendo propiedades con filtros: name={Name}, address={Address}, minPrice={MinPrice}, maxPrice={MaxPrice}";
		public const string PropiedadesObtenidas = "Se obtuvieron {Count} propiedades.";
		public const string ErrorObtenerPropiedades = "Error al obtener propiedades con filtros: name={Name}, address={Address}, minPrice={MinPrice}, maxPrice={MaxPrice}";

		public const string ConsultarPropiedadPorId = "Consultando propiedad con ID: {Id}";
		public const string PropiedadNoEncontrada = "No se encontró la propiedad con ID: {Id}";
		public const string ErrorObtenerPropiedadPorId = "Error al obtener propiedad con ID: {Id}";

		// ========== LOGS PARA CREACIÓN ==========
		public const string IntentandoCrearPropiedad = "Intentando crear propiedad para el IdOwner: {IdOwner}";
		public const string IdOwnerNoExiste = "No se puede crear propiedad. IdOwner '{IdOwner}' no existe.";
		public const string PropiedadCreada = "Propiedad creada exitosamente para el IdOwner: {IdOwner}";
		public const string ErrorCrearPropiedad = "Error al crear propiedad para el IdOwner: {IdOwner}";

		// ====== RESPUESTAS EXITOSAS ======
		public const string PropiedadesObtenidasExito = "Propiedades obtenidas correctamente";

		// ====== RESPUESTAS DE ERROR ======
		public const string ErrorObtenerPropiedadesMensaje = "Ocurrió un error al obtener las propiedades.";

		// ====== RESPUESTAS PARA CONSULTA POR ID ======
		public const string PropiedadNoExisteMensaje = "La propiedad no existe.";
		public const string ErrorConsultarPropiedadMensaje = "Ocurrió un error al consultar la propiedad.";
		public const string PropiedadObtenidaMensaje = "Propiedad obtenida correctamente";

		// ====== RESPUESTAS PARA CREACIÓN DE PROPIEDAD ======
		public const string IdOwnerNoExisteMensaje = "No se puede crear la propiedad. El IdOwner '{0}' no existe.";
		public const string PropiedadCreadaMensaje = "Propiedad creada exitosamente";
		public const string ErrorCrearPropiedadMensaje = "Ocurrió un error al crear la propiedad.";



		// ========== LOGS PARA IMAGENES ==========
		public const string ConsultarImagenesPorPropiedad = "Consultando imágenes para la propiedad con ID: {IdProperty}";
		public const string ImagenesEncontradas = "Se encontraron {Count} imágenes para la propiedad {IdProperty}";
		public const string ErrorConsultarImagenes = "Error al obtener imágenes para la propiedad con ID: {IdProperty}";

		// ========== LOGS PARA PROPIEDADES ==========
		public const string FiltroPropiedades = "Se consultaron {Count} propiedades con filtros";
		public const string ErrorFiltroPropiedades = "Error al consultar propiedades con filtros";

		public const string PropiedadIdInvalido = "ID de propiedad inválido: {Id}";
		public const string PropiedadEncontrada = "Se obtuvo propiedad con ID: {Id}";
		public const string ErrorObtenerPropiedad = "Error al obtener propiedad con ID: {Id}";

		public const string PropiedadInsertada = "Propiedad insertada correctamente con ID: {IdOwner}";
		public const string ErrorInsertarPropiedad = "Error al insertar propiedad para el IdOwner: {IdOwner}";

		// ===== LOGS PARA OWNER =====
		public const string IdOwnerInvalido = "IdOwner '{IdOwner}' tiene un formato inválido.";
		public const string VerificacionExistenciaOwner = "Verificación de existencia para IdOwner '{IdOwner}': {Existe}";
		public const string ErrorVerificarExistenciaOwner = "Error al verificar existencia de IdOwner: {IdOwner}";

		public const string OwnerObtenidoPorId = "Se obtuvo el Owner con Id: {0}";
		public const string ErrorObtenerOwnerPorId = "Error al obtener el Owner con Id: {0}";
		public const string OwnersObtenidos = "Se obtuvieron todos los Owners.";
		public const string ErrorObtenerTodosOwners = "Error al obtener todos los Owners.";
		public const string OwnerCreado = "Se creó el Owner: {0}";
		public const string ErrorCrearOwner = "Error al crear el Owner: {0}";
		public const string OwnerActualizado = "Se actualizó el Owner con Id: {0}";
		public const string ErrorActualizarOwner = "Error al actualizar el Owner con Id: {0}";
		public const string OwnerEliminado = "Se eliminó el Owner con Id: {0}";
		public const string ErrorEliminarOwner = "Error al eliminar el Owner con Id: {0}";

		//OWNER

		public const string PropietariosObtenidos = "Lista de propietarios obtenida exitosamente.";
		public const string PropietarioObtenido = "Propietario obtenido correctamente.";
		public const string PropietarioNoEncontrado = "El propietario no existe.";
		public const string PropietarioCreado = "Propietario creado correctamente.";
		public const string PropietarioActualizado = "Propietario actualizado correctamente.";
		public const string PropietarioEliminado = "Propietario eliminado correctamente.";

		public const string ErrorObtenerPropietarios = "Error al obtener los propietarios.";
		public const string ErrorObtenerPropietario = "Error al obtener el propietario.";
		public const string ErrorCrearPropietario = "Error al crear el propietario.";
		public const string ErrorActualizarPropietario = "Error al actualizar el propietario.";
		public const string ErrorEliminarPropietario = "Error al eliminar el propietario.";
		public const string OwnerNoActualizado = "No se actualizó el propietario. ID: {0}";

		// PROPERTY

		public const string ErrorActualizarPropiedad = "Error al actualizar propiedad con ID {0}";
		public const string ErrorActualizarPropiedadMensaje = "Error al actualizar la propiedad.";
		public const string PropiedadActualizada = "Propiedad actualizada con ID {0}";
		public const string PropiedadActualizadaMensaje = "Propiedad actualizada correctamente.";

		public const string ErrorEliminarPropiedad = "Error al eliminar propiedad con ID {0}";
		public const string ErrorEliminarPropiedadMensaje = "Error al eliminar la propiedad.";
		public const string PropiedadEliminada = "Propiedad eliminada con ID {0}";
		public const string PropiedadEliminadaMensaje = "Propiedad eliminada correctamente.";

		//PROPERTY TRACE


		public const string TrazaCreadaMensaje = "Traza registrada correctamente.";
		public const string TrazaEliminadaMensaje = "Traza eliminada correctamente.";
		public const string TrazaNoExisteMensaje = "No se encontró la traza.";
		public const string TrazaObtenidaMensaje = "Trazas obtenidas correctamente.";
		public const string ErrorCrearTraza = "Error al registrar la traza.";
		public const string ErrorEliminarTraza = "Error al eliminar la traza.";
		public const string ErrorObtenerTraza = "Error al obtener las trazas.";

		public const string TrazaCreadaLog = "Traza creada correctamente para la propiedad {IdProperty}";
		public const string ErrorCrearTrazaLog = "Error al crear la traza";

		public const string TrazaNoEncontradaEliminarLog = "No se encontró la traza con ID {Id} para eliminar";
		public const string TrazaEliminadaLog = "Traza eliminada con ID {Id}";
		public const string ErrorEliminarTrazaLog = "Error al eliminar la traza";

		public const string TrazasEncontradasLog = "Se encontraron {Count} trazas para la propiedad {IdProperty}";
		public const string ErrorObtenerTrazasLog = "Error al obtener trazas para la propiedad {IdProperty}";

		//PROPERTY IMAGE


		public const string ImagenCreada = "Imagen {0} creada correctamente.";
		public const string ImagenCreadaCorrectamente = "Imagen registrada correctamente.";
		public const string ErrorCrearImagen = "Error al registrar la imagen.";

		public const string ImagenEliminada = "Imagen con ID {0} eliminada correctamente.";
		public const string ImagenEliminadaCorrectamente = "Imagen eliminada correctamente.";
		public const string ImagenNoEncontradaEliminar = "No se encontró la imagen a eliminar.";
		public const string ErrorEliminarImagen = "Error al eliminar la imagen.";

		public const string ImagenesObtenidasCorrectamente = "Imágenes obtenidas correctamente.";
		public const string ErrorObtenerImagenes = "Error al obtener las imágenes.";
		// PROPERY COMPLETA

		public const string ErrorSubirImagen = "No se pudo subir la imagen";
		public const string MensajeRegistroExitoso = "Registro completo exitoso";
		public const string MensajeErrorRegistro = "Error en el registro: ";
		public const string ImagenPropietario = "imagenes-propietarios";
		public const string ImagenesPropiedades = "imagenes-propiedades";
		public const string ErrorInternoServidor = "Error interno del servidor.";

		// Mensajes para respuestas al cliente

		public const string ValidacionModeloInvalido = "Los datos enviados no son válidos.";
		public const string CreacionExitosa = "Propiedad creada exitosamente.";
		public const string ActualizacionExitosa = "Propiedad actualizada exitosamente.";
		public const string EliminacionExitosa = "Propiedad eliminada exitosamente.";
		public const string RegistroCompletoExitoso = "Registro completo realizado correctamente.";
		public const string ErrorValidacionDatos = "Error en la validación de datos.";

		// Textos para logs (mensajes internos)
		public const string LogErrorObtenerPropiedad = "Error en ObtenerPropiedad";
		public const string LogErrorObtenerPorId = "Error en ObtenerPorId con id: {Id}";
		public const string LogErrorCrear = "Error en Crear propiedad";
		public const string LogErrorEliminar = "Error en Eliminar propiedad con id: {Id}";
		public const string LogErrorActualizar = "Error en Actualizar propiedad con id: {Id}";
		public const string LogErrorRegistrarPropiedadCompleta = "Error en RegistrarPropiedadCompleta";

		// Mensajes para respuestas

		public const string PropietarioNoExisteMensaje = "El propietario no existe.";


		// Mensajes para logs
		public const string LogErrorObtenerTodosPropietarios = "Error en ObtenerTodos propietarios";
		public const string LogErrorObtenerPropietarioPorId = "Error en ObtenerPorId propietario con id: {Id}";
		public const string LogErrorCrearPropietario = "Error en Crear propietario";
		public const string LogErrorActualizarPropietario = "Error en Actualizar propietario con id: {Id}";
		public const string LogErrorEliminarPropietario = "Error en Eliminar propietario con id: {Id}";

		// Mensajes para respuestas

		public const string ImagenNoExisteMensaje = "La imagen no existe.";

		// Mensajes para logs
		public const string LogErrorObtenerImagenPorPropiedad = "Error en ObtenerPorIdPropiedad con idProperty: {0}";
		public const string LogErrorCrearImagenPropiedad = "Error en Crear imagen de propiedad";
		public const string LogErrorEliminarImagenPropiedad = "Error en Eliminar imagen de propiedad con id: {0}";


		public const string TrazoNoExisteMensaje = "El trazo no existe.";

		public const string LogErrorObtenerTrazoPorPropiedad = "Error en ObtenerPorIdPropiedad con idProperty: {0}";
		public const string LogErrorCrearTrazoPropiedad = "Error en Crear trazo de propiedad";
		public const string LogErrorEliminarTrazoPropiedad = "Error en Eliminar trazo de propiedad con id: {0}";
	}

}










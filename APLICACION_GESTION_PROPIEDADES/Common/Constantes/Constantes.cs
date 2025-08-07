namespace APLICACION_GESTION_PROPIEDADES.Common.Constantes
{

	public static class Constantes
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

		// ===== LOGS PARA OWNER =====
		public const string IdOwnerInvalido = "IdOwner '{IdOwner}' tiene un formato inválido.";
		public const string VerificacionExistenciaOwner = "Verificación de existencia para IdOwner '{IdOwner}': {Existe}";
		public const string ErrorVerificarExistenciaOwner = "Error al verificar existencia de IdOwner: {IdOwner}";

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
	}

}


namespace APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio
{
	using DOMINIO_GESTION_PROPIEDADES.Entities;

	public interface IPropertyImageRepositorio
	{
		Task<List<PropertyImage>> ObtenerImagenesPorPropiedad(string idProperty);
	}

}

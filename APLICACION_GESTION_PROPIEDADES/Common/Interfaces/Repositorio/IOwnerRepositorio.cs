namespace APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio
{
	public interface IOwnerRepositorio
	{
		Task<bool> ExisteOwner(string idOwner);
	}
}


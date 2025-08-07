using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio
{
	public interface IOwnerRepositorio
	{
		Task<bool> ExisteOwner(string idOwner);
	}
}


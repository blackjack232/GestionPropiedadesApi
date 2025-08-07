using AutoMapper;

namespace APLICACION_GESTION_PROPIEDADES.Common.perfiles
{

	public class MappingProfile : Profile
	{
		public MappingProfile()
		{

		}

		protected internal MappingProfile(string profileName) : base(profileName)
		{
		}

		protected internal MappingProfile(string profileName,
			Action<IProfileExpression> configurationAction) : base(profileName,
			configurationAction)
		{
		}
	}
}

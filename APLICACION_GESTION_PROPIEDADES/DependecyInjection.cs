using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Common.perfiles;
using APLICACION_GESTION_PROPIEDADES.Common.Transversales;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Servicios;
using APLICACION_GESTION_PROPIEDADES.Servicios.APLICACION_GESTION_PROPIEDADES.Servicios;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace APLICACION_GESTION_PROPIEDADES
{
	public static class DependecyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			// Configura FluentValidation para validación automática y adaptadores del lado del cliente
			services.AddFluentValidationAutoValidation()
					.AddFluentValidationClientsideAdapters();

			// Configura AutoMapper usando el perfil de mapeo definido
			services.AddAutoMapper(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});

			// Inyección de dependencias para la capa de aplicación
			services.AddTransient<IPropertyAplicacion, PropertyServicio>();
			services.AddTransient<IOwnerAplicacion, OwnerServicio>();
			services.AddTransient<IPropertyImageAplicacion, PropertyImageServicio>();
			services.AddTransient<IPropertyTraceAplicacion, PropertyTraceServicio>();
			services.AddScoped<ICloudinaryServicio, CloudinaryServicio>();

			return services;
		}
	}
}

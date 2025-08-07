using System.ComponentModel.DataAnnotations;

namespace APLICACION_GESTION_PROPIEDADES.Validaciones
{

	public static class ValidacionFecha
	{
		public static ValidationResult NoEnElFuturo(DateTime date, ValidationContext context)
		{
			if (date > DateTime.Today)
				return new ValidationResult("La fecha no puede estar en el futuro.");
			return ValidationResult.Success;
		}
	}

}

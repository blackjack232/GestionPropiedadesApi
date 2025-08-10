using System.Text;

namespace APLICACION_GESTION_PROPIEDADES.Common.Utils
{
	public static class ConnectionStringProtector
	{

		public static string Encode(string input)
		{
			if (string.IsNullOrEmpty(input))
				return string.Empty;

			byte[] bytes = Encoding.UTF8.GetBytes(input);
			return Convert.ToBase64String(bytes);
		}

		// Método para "desencriptar" (decodificar)
		public static string Decode(string encodedInput)
		{
			if (string.IsNullOrEmpty(encodedInput))
				return string.Empty;

			try
			{
				byte[] bytes = Convert.FromBase64String(encodedInput);
				return Encoding.UTF8.GetString(bytes);
			}
			catch (FormatException)
			{
				// Si no es Base64 válido, devuelve la cadena original
				return encodedInput;
			}
		}
	}
}
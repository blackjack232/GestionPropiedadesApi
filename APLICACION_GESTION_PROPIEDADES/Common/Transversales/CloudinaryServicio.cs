using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace APLICACION_GESTION_PROPIEDADES.Common.Transversales
{
	public class CloudinaryServicio : ICloudinaryServicio
	{
		private readonly Cloudinary _cloudinary;

		public CloudinaryServicio(IConfiguration configuration)
		{
			var account = new Account(
				configuration["Cloudinary:CloudName"],
				configuration["Cloudinary:ApiKey"],
				configuration["Cloudinary:ApiSecret"]
			);

			_cloudinary = new Cloudinary(account);
		}

		public async Task<string?> SubirImagen(IFormFile imagen, string file)
		{
			if (imagen == null || imagen.Length == 0)
				return null;

			await using var stream = imagen.OpenReadStream();

			var uploadParams = new ImageUploadParams
			{
				File = new FileDescription(imagen.FileName, stream),
				Folder = file
			};

			var uploadResult = await _cloudinary.UploadAsync(uploadParams);

			if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
				return uploadResult.SecureUrl.ToString();

			return null;
		}


	}
}

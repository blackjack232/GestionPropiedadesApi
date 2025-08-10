namespace APLICACION_TEST
{
	using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
	using APLICACION_GESTION_PROPIEDADES.Dto.Request;
	using APLICACION_GESTION_PROPIEDADES.Interfaces.Repositorio;
	using APLICACION_GESTION_PROPIEDADES.Servicios;
	using DOMINIO_GESTION_PROPIEDADES.Entities;
	using Microsoft.Extensions.Logging;
	using Moq;
	using NUnit.Framework;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	[TestFixture]
	public class PropertyImageServicioTests
	{
		private Mock<IPropertyImageRepositorio> _mockImageRepo;
		private Mock<IPropertyRespositorio> _mockPropiedadRepo;
		private Mock<ILogger<PropertyImageServicio>> _mockLogger;
		private PropertyImageServicio _service;

		[SetUp]
		public void Setup()
		{
			_mockImageRepo = new Mock<IPropertyImageRepositorio>();
			_mockPropiedadRepo = new Mock<IPropertyRespositorio>();
			_mockLogger = new Mock<ILogger<PropertyImageServicio>>();
			_service = new PropertyImageServicio(_mockImageRepo.Object, _mockPropiedadRepo.Object, _mockLogger.Object);
		}

		[Test]
		public async Task Crear_ReturnsFail_WhenPropiedadNotFound()
		{
			// Arrange
			var request = new PropertyImageRequest { IdProperty = "prop123", File = "file1", Enabled = true };
			_mockPropiedadRepo.Setup(r => r.ObtenerPorId(request.IdProperty)).ReturnsAsync((Property)null);

			// Act
			var result = await _service.Crear(request);

			// Assert
			Assert.IsFalse(result.Success);
			Assert.AreEqual("La propiedad no existe.", result.Message);
		}

		[Test]
		public async Task Crear_ReturnsOk_WhenPropiedadExists()
		{
			// Arrange
			var request = new PropertyImageRequest { IdProperty = "prop123", File = "file1", Enabled = true };
			var propiedad = new Property { IdProperty = "prop123" };

			_mockPropiedadRepo.Setup(r => r.ObtenerPorId(request.IdProperty)).ReturnsAsync(propiedad);
			_mockImageRepo.Setup(r => r.Crear(It.IsAny<PropertyImage>())).Returns(Task.CompletedTask);

			// Act
			var result = await _service.Crear(request);

			// Assert
			Assert.IsTrue(result.Success);
			Assert.AreEqual("Imagen registrada correctamente.", result.Message);

			_mockImageRepo.Verify(r => r.Crear(It.Is<PropertyImage>(p =>
				p.IdProperty == request.IdProperty &&
				p.File == request.File &&
				p.Enabled == request.Enabled)), Times.Once);
		}

		[Test]
		public async Task Eliminar_ReturnsFail_WhenNotDeleted()
		{
			// Arrange
			string id = "img123";
			_mockImageRepo.Setup(r => r.Eliminar(id)).ReturnsAsync(false);

			// Act
			var result = await _service.Eliminar(id);

			// Assert
			Assert.IsFalse(result.Success);
			Assert.AreEqual("No se encontró la imagen a eliminar.", result.Message);
		}

		[Test]
		public async Task Eliminar_ReturnsOk_WhenDeleted()
		{
			// Arrange
			string id = "img123";
			_mockImageRepo.Setup(r => r.Eliminar(id)).ReturnsAsync(true);

			// Act
			var result = await _service.Eliminar(id);

			// Assert
			Assert.IsTrue(result.Success);
			Assert.AreEqual("Imagen eliminada correctamente.", result.Message);
		}

		[Test]
		public async Task ObtenerPorIdPropiedad_ReturnsListOfImages()
		{
			// Arrange
			string idProperty = "prop123";
			var images = new List<PropertyImage>
		{
			new PropertyImage { IdPropertyImage = "img1", IdProperty = idProperty, File = "file1", Enabled = true },
			new PropertyImage { IdPropertyImage = "img2", IdProperty = idProperty, File = "file2", Enabled = false }
		};
			_mockImageRepo.Setup(r => r.ObtenerPorIdPropiedad(idProperty)).ReturnsAsync(images);

			// Act
			var result = await _service.ObtenerPorIdPropiedad(idProperty);

			// Assert
			Assert.IsTrue(result.Success);
			Assert.IsNotNull(result.Data);
			Assert.AreEqual(2, result.Data.Count());

			var first = result.Data.First();
			Assert.AreEqual("img1", first.IdPropertyImage);
			Assert.AreEqual("file1", first.File);
		}
	}

}

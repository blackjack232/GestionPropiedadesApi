using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Dto.Request.APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Servicios;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace APLICACION_TEST
{
	[TestFixture]
	public class PropiedadServicioTests
	{
		private Mock<IPropertyRespositorio> _mockPropiedadRepo;
		private Mock<IOwnerRepositorio> _mockOwnerRepo;
		private Mock<IPropertyImageRepositorio> _mockImageRepo;
		private Mock<ILogger<PropertyServicio>> _mockLogger;
		private PropertyServicio _servicio;

		[SetUp]
		public void SetUp()
		{
			_mockPropiedadRepo = new Mock<IPropertyRespositorio>();
			_mockOwnerRepo = new Mock<IOwnerRepositorio>();
			_mockImageRepo = new Mock<IPropertyImageRepositorio>();
			_mockLogger = new Mock<ILogger<PropertyServicio>>();

			_servicio = new PropertyServicio(
				_mockPropiedadRepo.Object,
				_mockOwnerRepo.Object,
				_mockImageRepo.Object,
				_mockLogger.Object
			);
		}

		#region ObtenerPropiedad

		[Test]
		public async Task ObtenerPropiedad_DeberiaRetornarListaDeDto()
		{
			// Arrange
			var properties = new List<Property> {
				new Property { IdProperty = "123", IdOwner = "abc", Name = "Casa", Address = "Calle 1", Price = 1000000 }
			};
			var images = new List<PropertyImage> {
				new PropertyImage { IdProperty = "123", File = "imagen.jpg", Enabled = true }
			};

			_mockPropiedadRepo.Setup(r => r.ObtenerPropiedad(null, null, null, null)).ReturnsAsync(properties);
			_mockImageRepo.Setup(r => r.ObtenerImagenesPorPropiedad("123")).ReturnsAsync(images);

			// Act
			var resultado = await _servicio.ObtenerPropiedad(null, null, null, null);

			// Assert
			Assert.IsTrue(resultado.Success);
			Assert.AreEqual(1, resultado.Data?.Count());
		}

		[Test]
		public async Task ObtenerPropiedad_CuandoFallaDebeRetornarFail()
		{
			_mockPropiedadRepo.Setup(r => r.ObtenerPropiedad(It.IsAny<string>(), null, null, null))
							  .ThrowsAsync(new Exception("DB error"));

			var resultado = await _servicio.ObtenerPropiedad("Casa", null, null, null);

			Assert.IsFalse(resultado.Success);
			Assert.AreEqual(Constantes.ErrorObtenerPropiedadesMensaje, resultado.Message);
		}

		#endregion

		#region ObtenerPorId

		[Test]
		public async Task ObtenerPorId_DeberiaRetornarDtoSiExiste()
		{
			var propiedad = new Property
			{
				IdProperty = "123",
				IdOwner = "abc",
				Name = "Casa",
				Address = "Calle 1",
				Price = 1000000
			};
			var imagenes = new List<PropertyImage> {
				new PropertyImage { IdProperty = "123", File = "foto.jpg", Enabled = true }
			};

			_mockPropiedadRepo.Setup(r => r.ObtenerPorId("123")).ReturnsAsync(propiedad);
			_mockImageRepo.Setup(r => r.ObtenerImagenesPorPropiedad("123")).ReturnsAsync(imagenes);

			var resultado = await _servicio.ObtenerPorId("123");

			Assert.IsTrue(resultado.Success);
			Assert.IsNotNull(resultado.Data);
			Assert.AreEqual("Casa", resultado.Data?.Name);
		}

		[Test]
		public async Task ObtenerPorId_DeberiaRetornarFailSiNoExiste()
		{
			_mockPropiedadRepo.Setup(r => r.ObtenerPorId("999")).ReturnsAsync((Property?)null);

			var resultado = await _servicio.ObtenerPorId("999");

			Assert.IsFalse(resultado.Success);
			Assert.AreEqual(Constantes.PropiedadNoExisteMensaje, resultado.Message);
		}

		[Test]
		public async Task ObtenerPorId_CuandoFallaDebeRetornarFail()
		{
			_mockPropiedadRepo.Setup(r => r.ObtenerPorId(It.IsAny<string>())).ThrowsAsync(new Exception("Error interno"));

			var resultado = await _servicio.ObtenerPorId("123");

			Assert.IsFalse(resultado.Success);
			Assert.AreEqual(Constantes.ErrorConsultarPropiedadMensaje, resultado.Message);
		}

		#endregion

		#region Crear

		[Test]
		public async Task Crear_DeberiaCrearSiIdOwnerExiste()
		{
			var dto = new PropertyRequest
			{
				IdOwner = "abc",
				Name = "Casa",
				Address = "Calle 1",
				Price = 1000000,
				CodeInternal = "INT001",
				Year = 2022
			};

			_mockOwnerRepo.Setup(r => r.ExisteOwner("abc")).ReturnsAsync(true);

			var resultado = await _servicio.Crear(dto);

			Assert.IsTrue(resultado.Success);
			Assert.AreEqual(Constantes.PropiedadCreadaMensaje, resultado.Message);
		}

		[Test]
		public async Task Crear_DeberiaRetornarFailSiIdOwnerNoExiste()
		{
			var dto = new PropertyRequest { IdOwner = "xyz" };
			_mockOwnerRepo.Setup(r => r.ExisteOwner("xyz")).ReturnsAsync(false);

			var resultado = await _servicio.Crear(dto);

			Assert.IsFalse(resultado.Success);
			Assert.AreEqual(string.Format(Constantes.IdOwnerNoExisteMensaje, "xyz"), resultado.Message);
		}

		[Test]
		public async Task Crear_CuandoFallaDebeRetornarFail()
		{
			var dto = new PropertyRequest { IdOwner = "abc" };

			_mockOwnerRepo.Setup(r => r.ExisteOwner("abc")).ReturnsAsync(true);
			_mockPropiedadRepo.Setup(r => r.Crear(It.IsAny<Property>())).ThrowsAsync(new Exception("Falla"));

			var resultado = await _servicio.Crear(dto);

			Assert.IsFalse(resultado.Success);
			Assert.AreEqual(Constantes.ErrorCrearPropiedadMensaje, resultado.Message);
		}

		#endregion
	}
}
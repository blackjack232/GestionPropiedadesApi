using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Common.Transversales;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Request.APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace API_TEST
{
	[TestFixture]
	public class PropertyServicioTests
	{
		private Mock<IPropertyRespositorio> _mockPropiedadRepositorio;
		private Mock<IOwnerRepositorio> _mockOwnerRepositorio;
		private Mock<IPropertyImageRepositorio> _mockImageRepositorio;
		private Mock<ICloudinaryServicio> _mockCloudinaryServicio;

		private Mock<ILogger<PropertyServicio>> _mockLogger;

		private PropertyServicio _service;
		private CloudinaryServicio _cloudinaryServicio;


		[SetUp]
		public void Setup()
		{
			_mockPropiedadRepositorio = new Mock<IPropertyRespositorio>();
			_mockOwnerRepositorio = new Mock<IOwnerRepositorio>();
			_mockImageRepositorio = new Mock<IPropertyImageRepositorio>();
			_mockCloudinaryServicio = new Mock<ICloudinaryServicio>();

			_mockLogger = new Mock<ILogger<PropertyServicio>>();

			_service = new PropertyServicio(
				_mockPropiedadRepositorio.Object,
				_mockOwnerRepositorio.Object,
				_mockImageRepositorio.Object,
				_mockLogger.Object,
				_mockCloudinaryServicio.Object
			);

		}

		#region ObtenerPropiedad

		[Test]
		public async Task ObtenerPropiedad_RetornaOkConDatos()
		{
			var propertiesMock = new List<DOMINIO_GESTION_PROPIEDADES.Entities.Property>
			{
				new DOMINIO_GESTION_PROPIEDADES.Entities.Property { IdProperty = "1", IdOwner = "owner1", Name = "Casa 1", Address = "Calle 1", Price = 100 }
			};
			int totalCountMock = 1;

			_mockPropiedadRepositorio
				.Setup(r => r.ObtenerPropiedadPaginada(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync((propertiesMock, totalCountMock));

			_mockImageRepositorio
				.Setup(r => r.ObtenerImagenesPorPropiedades(It.IsAny<List<string>>()))
				.ReturnsAsync(new List<DOMINIO_GESTION_PROPIEDADES.Entities.PropertyImage>());

			var result = await _service.ObtenerPropiedad(null, null, null, null);

			Assert.IsTrue(result.Success);
			Assert.IsNotNull(result.Data);
			Assert.AreEqual(totalCountMock, result.Data.TotalRecords);

		}

		[Test]
		public async Task ObtenerPropiedad_RetornaFailEnExcepcion()
		{
			_mockPropiedadRepositorio
				.Setup(r => r.ObtenerPropiedadPaginada(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<int>(), It.IsAny<int>()))
				.ThrowsAsync(new Exception("error"));

			var result = await _service.ObtenerPropiedad(null, null, null, null);

			Assert.IsFalse(result.Success);
			Assert.IsNull(result.Data);
		}

		#endregion

		#region ObtenerPorId

		[Test]
		public async Task ObtenerPorId_RetornaOkConDato()
		{
			var propertyMock = new DOMINIO_GESTION_PROPIEDADES.Entities.Property
			{
				IdProperty = "1",
				IdOwner = "owner1",
				Name = "Casa 1",
				Address = "Direccion 1",
				Price = 123
			};

			var imagesMock = new List<DOMINIO_GESTION_PROPIEDADES.Entities.PropertyImage>
			{
				new DOMINIO_GESTION_PROPIEDADES.Entities.PropertyImage { File = "url1" }
			};

			_mockPropiedadRepositorio.Setup(r => r.ObtenerPorId("1")).ReturnsAsync(propertyMock);
			_mockImageRepositorio.Setup(r => r.ObtenerImagenesPorPropiedad("1")).ReturnsAsync(imagesMock);

			var result = await _service.ObtenerPorId("1");

			Assert.IsTrue(result.Success);
			Assert.IsNotNull(result.Data);
			Assert.AreEqual("1", propertyMock.IdProperty);
			Assert.AreEqual("url1", result.Data.ImageUrls[0]);
		}

		[Test]
		public async Task ObtenerPorId_RetornaFailSiNoExiste()
		{
			_mockPropiedadRepositorio.Setup(r => r.ObtenerPorId(It.IsAny<string>())).ReturnsAsync((DOMINIO_GESTION_PROPIEDADES.Entities.Property)null);

			var result = await _service.ObtenerPorId("no-existe");

			Assert.IsFalse(result.Success);
			Assert.IsNull(result.Data);
		}

		[Test]
		public async Task ObtenerPorId_RetornaFailEnExcepcion()
		{
			_mockPropiedadRepositorio.Setup(r => r.ObtenerPorId(It.IsAny<string>())).ThrowsAsync(new Exception("error"));

			var result = await _service.ObtenerPorId("1");

			Assert.IsFalse(result.Success);
			Assert.IsNull(result.Data);
		}

		#endregion

		#region Crear

		[Test]
		public async Task Crear_RetornaFailSiOwnerNoExiste()
		{
			_mockOwnerRepositorio.Setup(o => o.ExisteOwner(It.IsAny<string>())).ReturnsAsync(false);

			var dto = new PropertyRequest
			{
				IdOwner = "no-existe",
				Name = "Casa",
				Address = "Calle",
				Price = 1000,
				CodeInternal = "ABC",
				Year = 2023
			};

			var result = await _service.Crear(dto);

			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.Message.Contains("no existe"));
		}

		[Test]
		public async Task Crear_RetornaOkSiTodoBien()
		{
			_mockOwnerRepositorio.Setup(o => o.ExisteOwner(It.IsAny<string>())).ReturnsAsync(true);
			_mockPropiedadRepositorio.Setup(p => p.Crear(It.IsAny<DOMINIO_GESTION_PROPIEDADES.Entities.Property>()));

			var dto = new PropertyRequest
			{
				IdOwner = "owner1",
				Name = "Casa",
				Address = "Calle",
				Price = 1000,
				CodeInternal = "ABC",
				Year = 2023
			};

			var result = await _service.Crear(dto);

			Assert.IsTrue(result.Success);
		}

		[Test]
		public async Task Crear_RetornaFailEnExcepcion()
		{
			_mockOwnerRepositorio.Setup(o => o.ExisteOwner(It.IsAny<string>())).ReturnsAsync(true);
			_mockPropiedadRepositorio.Setup(p => p.Crear(It.IsAny<DOMINIO_GESTION_PROPIEDADES.Entities.Property>())).ThrowsAsync(new Exception("error"));

			var dto = new PropertyRequest
			{
				IdOwner = "owner1",
				Name = "Casa",
				Address = "Calle",
				Price = 1000,
				CodeInternal = "ABC",
				Year = 2023
			};

			var result = await _service.Crear(dto);

			Assert.IsFalse(result.Success);
		}

		#endregion

		#region Actualizar

		[Test]
		public async Task Actualizar_RetornaFailSiNoExiste()
		{
			_mockPropiedadRepositorio.Setup(r => r.ObtenerPorId(It.IsAny<string>())).ReturnsAsync((DOMINIO_GESTION_PROPIEDADES.Entities.Property)null);

			var dto = new PropertyRequest { IdOwner = "owner", Name = "Name", Address = "Addr", Price = 100, CodeInternal = "C", Year = 2020 };

			var result = await _service.Actualizar("id-no-existe", dto);

			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.Message.Contains("no existe"));
		}

		//[Test]
		//public async Task Actualizar_RetornaFailSiFallaActualizar()
		//{
		//	_mockPropiedadRepositorio.Setup(r => r.ObtenerPorId(It.IsAny<string>())).ReturnsAsync(new DOMINIO_GESTION_PROPIEDADES.Entities.Property());
		//	_mockPropiedadRepositorio.Setup(r => r.Actualizar(It.IsAny<string>(), It.IsAny<DOMINIO_GESTION_PROPIEDADES.Entities.Property>())).ReturnsAsync(false);

		//	var dto = new PropertyRequest { IdOwner = "owner", Name = "Name", Address = "Addr", Price = 100, CodeInternal = "C", Year = 2020 };

		//	var result = await _service.Actualizar("id-existe", dto);

		//	Assert.IsFalse(result.Success);
		//	Assert.IsTrue(result.Message.Contains("error"));
		//}

		[Test]
		public async Task Actualizar_RetornaOkSiActualiza()
		{
			_mockPropiedadRepositorio.Setup(r => r.ObtenerPorId(It.IsAny<string>())).ReturnsAsync(new DOMINIO_GESTION_PROPIEDADES.Entities.Property());
			_mockPropiedadRepositorio.Setup(r => r.Actualizar(It.IsAny<string>(), It.IsAny<DOMINIO_GESTION_PROPIEDADES.Entities.Property>())).ReturnsAsync(true);

			var dto = new PropertyRequest { IdOwner = "owner", Name = "Name", Address = "Addr", Price = 100, CodeInternal = "C", Year = 2020 };

			var result = await _service.Actualizar("id-existe", dto);

			Assert.IsTrue(result.Success);
		}

		[Test]
		public async Task Actualizar_RetornaFailEnExcepcion()
		{
			_mockPropiedadRepositorio.Setup(r => r.ObtenerPorId(It.IsAny<string>())).ThrowsAsync(new Exception("error"));

			var dto = new PropertyRequest { IdOwner = "owner", Name = "Name", Address = "Addr", Price = 100, CodeInternal = "C", Year = 2020 };

			var result = await _service.Actualizar("id-existe", dto);

			Assert.IsFalse(result.Success);
		}

		#endregion

		#region Eliminar

		[Test]
		public async Task Eliminar_RetornaFailSiNoExiste()
		{
			_mockPropiedadRepositorio.Setup(r => r.Eliminar(It.IsAny<string>())).ReturnsAsync(false);

			var result = await _service.Eliminar("id-no-existe");

			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.Message.Contains("no existe"));
		}

		[Test]
		public async Task Eliminar_RetornaOkSiElimina()
		{
			_mockPropiedadRepositorio.Setup(r => r.Eliminar(It.IsAny<string>())).ReturnsAsync(true);

			var result = await _service.Eliminar("id-existe");

			Assert.IsTrue(result.Success);
		}

		[Test]
		public async Task Eliminar_RetornaFailEnExcepcion()
		{
			_mockPropiedadRepositorio.Setup(r => r.Eliminar(It.IsAny<string>())).ThrowsAsync(new Exception("error"));

			var result = await _service.Eliminar("id-existe");

			Assert.IsFalse(result.Success);
		}

		#endregion

		#region RegistrarPropiedadCompleta

		[Test]
		public async Task RegistrarPropiedadCompleta_RetornaOkSiTodoBien()
		{
			var request = new PropiedadCompletaRequest
			{
				Propietario = new OwnerRequest
				{
					Name = "Owner1",
					Address = "Address1",
					Birthday = DateTime.Now,
					Photo = CreateFakeFormFile()
				},
				Propiedad = new PropertyRequest
				{
					Name = "Casa 1",
					Address = "Calle 1",
					Price = 100,
					CodeInternal = "C123",
					Year = 2023,
					IdOwner = "owner1"
				},
				Imagen = CreateFakeFormFile()
			};

			_mockCloudinaryServicio
				.Setup(c => c.SubirImagen(It.IsAny<IFormFile>(), It.IsAny<string>()))
				.ReturnsAsync("http://fakeurl.com/image.jpg");

			_mockOwnerRepositorio
				.Setup(o => o.Crear(It.IsAny<OwnerDto>()))
				.ReturnsAsync("owner-id");

			_mockPropiedadRepositorio
				.Setup(p => p.Crear(It.IsAny<DOMINIO_GESTION_PROPIEDADES.Entities.Property>()))
				.ReturnsAsync("propiedad-id");

			_mockImageRepositorio
				.Setup(i => i.Crear(It.IsAny<DOMINIO_GESTION_PROPIEDADES.Entities.PropertyImage>()))
				.Returns(Task.CompletedTask);

			var result = await _service.RegistrarPropiedadCompleta(request);

			Assert.IsTrue(result.Success);
			Assert.AreEqual("propiedad-id", result.Data);
		}

		[Test]
		public async Task RegistrarPropiedadCompleta_FallaSiNoCreaOwner()
		{
			var request = new PropiedadCompletaRequest
			{
				Propietario = new OwnerRequest { Photo = CreateFakeFormFile() },
				Propiedad = new PropertyRequest(),
				Imagen = CreateFakeFormFile()
			};

			_mockCloudinaryServicio.Setup(c => c.SubirImagen(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync("url");

			_mockOwnerRepositorio.Setup(o => o.Crear(It.IsAny<OwnerDto>())).ReturnsAsync(string.Empty);

			var result = await _service.RegistrarPropiedadCompleta(request);

			Assert.IsFalse(result.Success);
		}

		[Test]
		public async Task RegistrarPropiedadCompleta_FallaSiNoCreaPropiedadYHaceRollback()
		{
			var request = new PropiedadCompletaRequest
			{
				Propietario = new OwnerRequest { Photo = CreateFakeFormFile() },
				Propiedad = new PropertyRequest(),
				Imagen = CreateFakeFormFile()
			};

			_mockCloudinaryServicio.Setup(c => c.SubirImagen(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync("url");

			_mockOwnerRepositorio.Setup(o => o.Crear(It.IsAny<OwnerDto>())).ReturnsAsync("ownerId");

			_mockPropiedadRepositorio.Setup(p => p.Crear(It.IsAny<DOMINIO_GESTION_PROPIEDADES.Entities.Property>())).ReturnsAsync(string.Empty);

			_mockOwnerRepositorio.Setup(o => o.Eliminar("ownerId"));

			var result = await _service.RegistrarPropiedadCompleta(request);

			Assert.IsFalse(result.Success);
			_mockOwnerRepositorio.Verify(o => o.Eliminar("ownerId"), Times.Once);
		}

		[Test]
		public async Task RegistrarPropiedadCompleta_FallaSiNoSubeImagenYHaceRollback()
		{
			var request = new PropiedadCompletaRequest
			{
				Propietario = new OwnerRequest { Photo = CreateFakeFormFile() },
				Propiedad = new PropertyRequest(),
				Imagen = CreateFakeFormFile()
			};

			_mockCloudinaryServicio.SetupSequence(c => c.SubirImagen(It.IsAny<IFormFile>(), It.IsAny<string>()))
				.ReturnsAsync("url") // para propietario
				.ReturnsAsync(string.Empty); // para propiedad (falla)

			_mockOwnerRepositorio.Setup(o => o.Crear(It.IsAny<OwnerDto>())).ReturnsAsync("ownerId");

			_mockPropiedadRepositorio.Setup(p => p.Crear(It.IsAny<DOMINIO_GESTION_PROPIEDADES.Entities.Property>())).ReturnsAsync("propiedadId");

			_mockPropiedadRepositorio.Setup(p => p.Eliminar("propiedadId"));
			_mockOwnerRepositorio.Setup(o => o.Eliminar("ownerId"));

			var result = await _service.RegistrarPropiedadCompleta(request);

			Assert.IsFalse(result.Success);
			_mockPropiedadRepositorio.Verify(p => p.Eliminar("propiedadId"), Times.Once);
			_mockOwnerRepositorio.Verify(o => o.Eliminar("ownerId"), Times.Once);
		}

		[Test]
		public async Task RegistrarPropiedadCompleta_FallaEnExcepcionYHaceRollback()
		{
			var request = new PropiedadCompletaRequest
			{
				Propietario = new OwnerRequest { Photo = CreateFakeFormFile() },
				Propiedad = new PropertyRequest(),
				Imagen = CreateFakeFormFile()
			};

			_mockCloudinaryServicio.Setup(c => c.SubirImagen(It.IsAny<IFormFile>(), It.IsAny<string>())).ThrowsAsync(new Exception("error"));

			var result = await _service.RegistrarPropiedadCompleta(request);

			Assert.IsFalse(result.Success);
		}

		#endregion

		// Helper para crear IFormFile falso para tests
		private static IFormFile CreateFakeFormFile()
		{
			var content = "Fake image content";
			var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
			return new FormFile(stream, 0, stream.Length, "Data", "fakeimage.jpg")
			{
				Headers = new HeaderDictionary(),
				ContentType = "image/jpeg"
			};
		}
	}
}

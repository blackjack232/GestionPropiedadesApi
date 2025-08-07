using API_GESTION_PROPIEDADES.Controllers.Propiedad;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Aplicacion;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API_TEST
{
	public class PropiedadControllerTests
	{
		private Mock<IPropiedadAplicacion> _mockServicio;
		private PropiedadController _controller;

		[SetUp]
		public void Setup()
		{
			_mockServicio = new Mock<IPropiedadAplicacion>();
			_controller = new PropiedadController(_mockServicio.Object);
		}

		[Test]
		public async Task ObtenerPropiedad_RetornaOk_SiServicioEsExitoso()
		{
			// Arrange
			var resultadoMock = ApiResponse<IEnumerable<PropertyDto>>.Ok(new List<PropertyDto>(), "ok");
			_mockServicio.Setup(s => s.ObtenerPropiedad(null, null, null, null))
						 .ReturnsAsync(resultadoMock);

			// Act
			var result = await _controller.ObtenerPropiedad(null, null, null, null);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(200, okResult!.StatusCode ?? 200);
		}

		[Test]
		public async Task ObtenerPropiedad_RetornaBadRequest_SiServicioFalla()
		{
			var response = ApiResponse<IEnumerable<PropertyDto>>.Fail("Error");
			_mockServicio.Setup(x => x.ObtenerPropiedad(It.IsAny<string>(), It.IsAny<string>(), null, null))
						 .ReturnsAsync(response);

			var result = await _controller.ObtenerPropiedad("test", "dir", null, null);

			Assert.IsInstanceOf<BadRequestObjectResult>(result);
		}

		[Test]
		public async Task ObtenerPorId_RetornaOk_SiPropiedadExiste()
		{
			var dto = new PropertyDto { IdOwner = "123", Name = "Casa A", Address = "Calle 123", Price = 1000000 };
			var response = ApiResponse<PropertyDto>.Ok(dto, "ok");

			_mockServicio.Setup(x => x.ObtenerPorId("1"))
						 .ReturnsAsync(response);

			var result = await _controller.ObtenerPorId("1");

			Assert.IsInstanceOf<OkObjectResult>(result);
		}

		[Test]
		public async Task ObtenerPorId_RetornaNotFound_SiPropiedadNoExiste()
		{
			var response = ApiResponse<PropertyDto?>.Fail("No encontrada");

			_mockServicio.Setup(x => x.ObtenerPorId("999"))
						 .ReturnsAsync(response);

			var result = await _controller.ObtenerPorId("999");

			Assert.IsInstanceOf<NotFoundObjectResult>(result);
		}

		[Test]
		public async Task Crear_RetornaOk_SiCreacionExitosa()
		{
			var dto = new PropertyDto { IdOwner = "123", Name = "Casa A", Address = "Calle 123", Price = 1000000 };
			var response = ApiResponse<string>.Ok(null, "Creada");

			_mockServicio.Setup(x => x.Crear(dto)).ReturnsAsync(response);

			var result = await _controller.Crear(dto);

			Assert.IsInstanceOf<OkObjectResult>(result);
		}

		[Test]
		public async Task Crear_RetornaBadRequest_SiFalla()
		{
			var dto = new PropertyDto { IdOwner = "123", Name = "Casa A", Address = "Calle 123", Price = 1000000 };
			var response = ApiResponse<string>.Fail("Error");

			_mockServicio.Setup(x => x.Crear(dto)).ReturnsAsync(response);

			var result = await _controller.Crear(dto);

			Assert.IsInstanceOf<BadRequestObjectResult>(result);
		}
	}
}
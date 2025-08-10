using API_GESTION_PROPIEDADES.Controllers.Propiedad;
using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request.APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Aplicacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace API_TEST
{
	public class PropertyControllerTests
	{
		private Mock<IPropertyAplicacion> _mockServicio = null!;
		private Mock<ILogger<PropertyController>> _mockLogger = null!;
		private PropertyController _controller = null!;

		[SetUp]
		public void Setup()
		{
			_mockServicio = new Mock<IPropertyAplicacion>();
			_mockLogger = new Mock<ILogger<PropertyController>>();
			_controller = new PropertyController(_mockServicio.Object, _mockLogger.Object);
		}

		[Test]
		public async Task ObtenerPropiedad_RetornaOk_SiServicioEsExitoso()
		{
			var pagedResponse = new PagedResponse<PropertyDto>
			{
				Data = new List<PropertyDto>(),
				PageNumber = 1,
				PageSize = 10
			};

			var resultadoMock = ApiResponse<PagedResponse<PropertyDto>>.Ok(pagedResponse, "ok");
			_mockServicio.Setup(s => s.ObtenerPropiedad(null, null, null, null, 1, 10))
						 .ReturnsAsync(resultadoMock);
			// Act
			var result = await _controller.ObtenerPropiedad(null, null, null, null);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			Assert.AreEqual(200, okResult!.StatusCode ?? 200);
		}
		[Test]
		public async Task ObtenerPropiedad_RetornaBadRequest_SiServicioFalla()
		{
			var pagedResponse = new PagedResponse<PropertyDto>
			{
				Data = new List<PropertyDto>(),
				PageNumber = 1,
				PageSize = 10,
				TotalPages = 0,

			};

			var response = ApiResponse<PagedResponse<PropertyDto>>.Fail("Error");

			_mockServicio.Setup(x => x.ObtenerPropiedad(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<int>(), It.IsAny<int>()))
						 .ReturnsAsync(response);

			var result = await _controller.ObtenerPropiedad("test", "dir", null, null, 1, 10);

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
			var response = ApiResponse<PropertyDto>.Fail("No encontrada");

			_mockServicio.Setup(x => x.ObtenerPorId("999"))
						 .ReturnsAsync(response);

			var result = await _controller.ObtenerPorId("999");

			Assert.IsInstanceOf<NotFoundObjectResult>(result);
		}

		[Test]
		public async Task Crear_RetornaOk_SiCreacionExitosa()
		{
			var dto = new PropertyRequest { IdOwner = "123", Name = "Casa A", Address = "Calle 123", Price = 1000000 };
			var response = ApiResponse<string>.Ok("nuevoId", MessageResponse.CreacionExitosa);

			_mockServicio.Setup(x => x.Crear(dto)).ReturnsAsync(response);

			var result = await _controller.Crear(dto);

			Assert.IsInstanceOf<OkObjectResult>(result);
		}

		[Test]
		public async Task Crear_RetornaBadRequest_SiFalla()
		{
			var dto = new PropertyRequest { IdOwner = "123", Name = "Casa A", Address = "Calle 123", Price = 1000000 };
			var response = ApiResponse<string>.Fail("Error");

			_mockServicio.Setup(x => x.Crear(dto)).ReturnsAsync(response);

			var result = await _controller.Crear(dto);

			Assert.IsInstanceOf<BadRequestObjectResult>(result);
		}

		[Test]
		public async Task Eliminar_RetornaOk_SiEliminacionExitosa()
		{
			var response = ApiResponse<string>.Ok("id123", MessageResponse.EliminacionExitosa);

			_mockServicio.Setup(x => x.Eliminar("id123")).ReturnsAsync(response);

			var result = await _controller.Eliminar("id123");

			Assert.IsInstanceOf<OkObjectResult>(result);
		}

		[Test]
		public async Task Eliminar_RetornaNotFound_SiNoExiste()
		{
			var response = ApiResponse<string>.Fail(MessageResponse.PropiedadNoExisteMensaje);

			_mockServicio.Setup(x => x.Eliminar("idNoExiste")).ReturnsAsync(response);

			var result = await _controller.Eliminar("idNoExiste");

			Assert.IsInstanceOf<NotFoundObjectResult>(result);
		}

		[Test]
		public async Task Actualizar_RetornaOk_SiActualizacionExitosa()
		{
			var dto = new PropertyRequest { IdOwner = "123", Name = "Casa Actualizada", Address = "Calle 456", Price = 1500000 };
			var response = ApiResponse<string>.Ok("id123", MessageResponse.ActualizacionExitosa);

			_mockServicio.Setup(x => x.Actualizar("id123", dto)).ReturnsAsync(response);

			var result = await _controller.Actualizar("id123", dto);

			Assert.IsInstanceOf<OkObjectResult>(result);
		}

		[Test]
		public async Task Actualizar_RetornaNotFound_SiNoExiste()
		{
			var dto = new PropertyRequest { IdOwner = "123", Name = "Casa Actualizada", Address = "Calle 456", Price = 1500000 };
			var response = ApiResponse<string>.Fail(MessageResponse.PropiedadNoExisteMensaje);

			_mockServicio.Setup(x => x.Actualizar("idNoExiste", dto)).ReturnsAsync(response);

			var result = await _controller.Actualizar("idNoExiste", dto);

			Assert.IsInstanceOf<NotFoundObjectResult>(result);
		}

		[Test]
		public async Task Actualizar_RetornaBadRequest_SiFalla()
		{
			var dto = new PropertyRequest { IdOwner = "123", Name = "Casa Actualizada", Address = "Calle 456", Price = 1500000 };
			var response = ApiResponse<string>.Fail("Error");

			_mockServicio.Setup(x => x.Actualizar("id123", dto)).ReturnsAsync(response);

			var result = await _controller.Actualizar("id123", dto);

			Assert.IsInstanceOf<BadRequestObjectResult>(result);
		}
	}
}

using API_GESTION_PROPIEDADES.Controllers.Owner;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace API_TEST
{
	[TestFixture]
	public class OwnerControllerTests
	{
		private Mock<IOwnerAplicacion> _mockServicio;
		private Mock<ILogger<OwnerController>> _mockLogger;
		private OwnerController _controller;

		[SetUp]
		public void Setup()
		{
			_mockServicio = new Mock<IOwnerAplicacion>();
			_mockLogger = new Mock<ILogger<OwnerController>>();
			_controller = new OwnerController(_mockServicio.Object, _mockLogger.Object);
		}

		[Test]
		public async Task ObtenerTodos_ReturnsOk_WhenSuccess()
		{
			// Arrange
			var owners = new List<OwnerResponse>
			{
				new OwnerResponse { Id = "1", Name = "Owner1" }
			};
			var response = ApiResponse<IEnumerable<OwnerResponse>>.Ok(owners);
			_mockServicio.Setup(s => s.ObtenerTodos()).ReturnsAsync(response);

			// Act
			var result = await _controller.ObtenerTodos();

			// Assert
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(200, okResult.StatusCode);

			var apiResponse = okResult.Value as ApiResponse<IEnumerable<OwnerResponse>>;
			Assert.IsTrue(apiResponse.Success);
			Assert.IsNotNull(apiResponse.Data);
		}

		[Test]
		public async Task ObtenerTodos_ReturnsBadRequest_WhenNotSuccess()
		{
			// Arrange
			var response = ApiResponse<IEnumerable<OwnerResponse>>.Fail("Error");
			_mockServicio.Setup(s => s.ObtenerTodos()).ReturnsAsync(response);

			// Act
			var result = await _controller.ObtenerTodos();

			// Assert
			var badRequest = result as BadRequestObjectResult;
			Assert.IsNotNull(badRequest);
			Assert.AreEqual(400, badRequest.StatusCode);
		}

		[Test]
		public async Task ObtenerTodos_ReturnsInternalServerError_OnException()
		{
			// Arrange
			_mockServicio.Setup(s => s.ObtenerTodos()).ThrowsAsync(new Exception("Error"));

			// Act
			var result = await _controller.ObtenerTodos();

			// Assert
			var objectResult = result as ObjectResult;
			Assert.IsNotNull(objectResult);
			Assert.AreEqual(500, objectResult.StatusCode);
		}

		[Test]
		public async Task ObtenerPorId_ReturnsOk_WhenFound()
		{
			// Arrange
			var owner = new OwnerResponse { Id = "1", Name = "Owner1" };
			var response = ApiResponse<OwnerResponse>.Ok(owner);
			_mockServicio.Setup(s => s.ObtenerPorId("1")).ReturnsAsync(response);

			// Act
			var result = await _controller.ObtenerPorId("1");

			// Assert
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(200, okResult.StatusCode);

			var apiResponse = okResult.Value as ApiResponse<OwnerResponse>;
			Assert.IsTrue(apiResponse.Success);
			Assert.IsNotNull(apiResponse.Data);
			Assert.AreEqual("1", apiResponse.Data.Id);
		}

		[Test]
		public async Task ObtenerPorId_ReturnsNotFound_WhenNotFound()
		{
			// Arrange
			var response = ApiResponse<OwnerResponse>.Fail("No existe");
			_mockServicio.Setup(s => s.ObtenerPorId("notfound")).ReturnsAsync(response);

			// Act
			var result = await _controller.ObtenerPorId("notfound");

			// Assert
			var notFoundResult = result as NotFoundObjectResult;
			Assert.IsNotNull(notFoundResult);
			Assert.AreEqual(404, notFoundResult.StatusCode);
		}

		[Test]
		public async Task ObtenerPorId_ReturnsInternalServerError_OnException()
		{
			// Arrange
			_mockServicio.Setup(s => s.ObtenerPorId(It.IsAny<string>())).ThrowsAsync(new Exception("Error"));

			// Act
			var result = await _controller.ObtenerPorId("1");

			// Assert
			var objectResult = result as ObjectResult;
			Assert.IsNotNull(objectResult);
			Assert.AreEqual(500, objectResult.StatusCode);
		}

		[Test]
		public async Task Crear_ReturnsOk_WhenSuccess()
		{
			// Arrange
			var dto = new OwnerDto { Name = "Owner1" };
			var response = ApiResponse<string>.Ok("id123", "Creación exitosa");
			_mockServicio.Setup(s => s.Crear(dto)).ReturnsAsync(response);

			// Act
			var result = await _controller.Crear(dto);

			// Assert
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(200, okResult.StatusCode);

			var apiResponse = okResult.Value as ApiResponse<string>;
			Assert.IsTrue(apiResponse.Success);
			Assert.AreEqual("id123", apiResponse.Data);
		}

		[Test]
		public async Task Crear_ReturnsBadRequest_WhenModelStateInvalid()
		{
			// Arrange
			_controller.ModelState.AddModelError("Name", "Required");

			// Act
			var result = await _controller.Crear(new OwnerDto());

			// Assert
			var badRequest = result as BadRequestObjectResult;
			Assert.IsNotNull(badRequest);
			Assert.AreEqual(400, badRequest.StatusCode);
		}

		[Test]
		public async Task Crear_ReturnsBadRequest_WhenServiceFails()
		{
			// Arrange
			var dto = new OwnerDto { Name = "Owner1" };
			var response = ApiResponse<string>.Fail("Error");
			_mockServicio.Setup(s => s.Crear(dto)).ReturnsAsync(response);

			// Act
			var result = await _controller.Crear(dto);

			// Assert
			var badRequest = result as BadRequestObjectResult;
			Assert.IsNotNull(badRequest);
			Assert.AreEqual(400, badRequest.StatusCode);
		}

		[Test]
		public async Task Crear_ReturnsInternalServerError_OnException()
		{
			// Arrange
			var dto = new OwnerDto { Name = "Owner1" };
			_mockServicio.Setup(s => s.Crear(dto)).ThrowsAsync(new Exception("Error"));

			// Act
			var result = await _controller.Crear(dto);

			// Assert
			var objectResult = result as ObjectResult;
			Assert.IsNotNull(objectResult);
			Assert.AreEqual(500, objectResult.StatusCode);
		}

		[Test]
		public async Task Actualizar_ReturnsOk_WhenSuccess()
		{
			// Arrange
			var dto = new OwnerDto { Name = "Owner1" };
			var response = ApiResponse<string>.Ok("id1", "Actualización exitosa");
			_mockServicio.Setup(s => s.Actualizar("id1", dto)).ReturnsAsync(response);

			// Act
			var result = await _controller.Actualizar("id1", dto);

			// Assert
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(200, okResult.StatusCode);
		}

		[Test]
		public async Task Actualizar_ReturnsBadRequest_WhenModelStateInvalid()
		{
			// Arrange
			_controller.ModelState.AddModelError("Name", "Required");

			// Act
			var result = await _controller.Actualizar("id1", new OwnerDto());

			// Assert
			var badRequest = result as BadRequestObjectResult;
		}
	}
}

using API_GESTION_PROPIEDADES.Controllers.PropertyImage;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Aplicacion;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace API_TEST
{
	[TestFixture]
	public class PropertyImageControllerTests
	{
		private Mock<IPropertyImageAplicacion> _mockService;
		private Mock<ILogger<PropertyImageController>> _mockLogger;
		private PropertyImageController _controller;

		[SetUp]
		public void Setup()
		{
			_mockService = new Mock<IPropertyImageAplicacion>();
			_mockLogger = new Mock<ILogger<PropertyImageController>>();
			_controller = new PropertyImageController(_mockService.Object, _mockLogger.Object);
		}

		[Test]
		public async Task ObtenerPorIdPropiedad_ReturnsOk_WhenSuccess()
		{
			// Arrange
			var images = new List<PropertyImageDto>
			{
				new PropertyImageDto { IdProperty = "img1", File = "url1" }
			};
			var response = ApiResponse<IEnumerable<PropertyImageDto>>.Ok(images);
			_mockService.Setup(s => s.ObtenerPorIdPropiedad("prop1")).ReturnsAsync(response);

			// Act
			var result = await _controller.ObtenerPorIdPropiedad("prop1");

			// Assert
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(200, okResult.StatusCode);

			var apiResponse = okResult.Value as ApiResponse<IEnumerable<PropertyImageDto>>;
			Assert.IsTrue(apiResponse.Success);
			Assert.IsNotNull(apiResponse.Data);
		}

		[Test]
		public async Task ObtenerPorIdPropiedad_ReturnsNotFound_WhenNotFound()
		{
			// Arrange
			var response = ApiResponse<IEnumerable<PropertyImageDto>>.Fail("No existe");
			_mockService.Setup(s => s.ObtenerPorIdPropiedad("propNotFound")).ReturnsAsync(response);

			// Act
			var result = await _controller.ObtenerPorIdPropiedad("propNotFound");

			// Assert
			var notFoundResult = result as NotFoundObjectResult;
			Assert.IsNotNull(notFoundResult);
			Assert.AreEqual(404, notFoundResult.StatusCode);
		}

		[Test]
		public async Task ObtenerPorIdPropiedad_ReturnsInternalServerError_OnException()
		{
			// Arrange
			_mockService.Setup(s => s.ObtenerPorIdPropiedad(It.IsAny<string>())).ThrowsAsync(new Exception("Error"));

			// Act
			var result = await _controller.ObtenerPorIdPropiedad("prop1");

			// Assert
			var objectResult = result as ObjectResult;
			Assert.IsNotNull(objectResult);
			Assert.AreEqual(500, objectResult.StatusCode);
		}

		[Test]
		public async Task Crear_ReturnsOk_WhenSuccess()
		{
			// Arrange
			var request = new PropertyImageRequest { IdProperty = "prop1", File = "url1" };
			var response = ApiResponse<string>.Ok("img1", "Creación exitosa");
			_mockService.Setup(s => s.Crear(request)).ReturnsAsync(response);

			// Act
			var result = await _controller.Crear(request);

			// Assert
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(200, okResult.StatusCode);

			var apiResponse = okResult.Value as ApiResponse<string>;
			Assert.IsTrue(apiResponse.Success);
			Assert.AreEqual("img1", apiResponse.Data);
		}

		[Test]
		public async Task Crear_ReturnsBadRequest_WhenModelStateInvalid()
		{
			// Arrange
			_controller.ModelState.AddModelError("Url", "Required");

			// Act
			var result = await _controller.Crear(new PropertyImageRequest());

			// Assert
			var badRequest = result as BadRequestObjectResult;
			Assert.IsNotNull(badRequest);
			Assert.AreEqual(400, badRequest.StatusCode);
		}

		[Test]
		public async Task Crear_ReturnsBadRequest_WhenServiceFails()
		{
			// Arrange
			var request = new PropertyImageRequest { IdProperty = "prop1", File = "url1" };
			var response = ApiResponse<string>.Fail("Error");
			_mockService.Setup(s => s.Crear(request)).ReturnsAsync(response);

			// Act
			var result = await _controller.Crear(request);

			// Assert
			var badRequest = result as BadRequestObjectResult;
			Assert.IsNotNull(badRequest);
			Assert.AreEqual(400, badRequest.StatusCode);
		}

		[Test]
		public async Task Crear_ReturnsInternalServerError_OnException()
		{
			// Arrange
			var request = new PropertyImageRequest { IdProperty = "prop1", File = "url1" };
			_mockService.Setup(s => s.Crear(request)).ThrowsAsync(new Exception("Error"));

			// Act
			var result = await _controller.Crear(request);

			// Assert
			var objectResult = result as ObjectResult;
			Assert.IsNotNull(objectResult);
			Assert.AreEqual(500, objectResult.StatusCode);
		}

		[Test]
		public async Task Eliminar_ReturnsOk_WhenSuccess()
		{
			// Arrange
			var response = ApiResponse<string>.Ok("img1", "Eliminación exitosa");
			_mockService.Setup(s => s.Eliminar("img1")).ReturnsAsync(response);

			// Act
			var result = await _controller.Eliminar("img1");

			// Assert
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(200, okResult.StatusCode);
		}

		[Test]
		public async Task Eliminar_ReturnsBadRequest_WhenServiceFails()
		{
			// Arrange
			var response = ApiResponse<string>.Fail("Error");
			_mockService.Setup(s => s.Eliminar("img1")).ReturnsAsync(response);

			// Act
			var result = await _controller.Eliminar("img1");

			// Assert
			var badRequest = result as BadRequestObjectResult;
			Assert.IsNotNull(badRequest);
			Assert.AreEqual(400, badRequest.StatusCode);
		}

		[Test]
		public async Task Eliminar_ReturnsInternalServerError_OnException()
		{
			// Arrange
			_mockService.Setup(s => s.Eliminar(It.IsAny<string>())).ThrowsAsync(new Exception("Error"));

			// Act
			var result = await _controller.Eliminar("img1");

			// Assert
			var objectResult = result as ObjectResult;
			Assert.IsNotNull(objectResult);
			Assert.AreEqual(500, objectResult.StatusCode);
		}
	}
}

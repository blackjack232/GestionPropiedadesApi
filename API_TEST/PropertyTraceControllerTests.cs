using API_GESTION_PROPIEDADES.Controllers.PropertyTrace;
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
	public class PropertyTraceControllerTests
	{
		private Mock<IPropertyTraceAplicacion> _mockService;
		private Mock<ILogger<PropertyTraceController>> _mockLogger;
		private PropertyTraceController _controller;

		[SetUp]
		public void Setup()
		{
			_mockService = new Mock<IPropertyTraceAplicacion>();
			_mockLogger = new Mock<ILogger<PropertyTraceController>>();
			_controller = new PropertyTraceController(_mockService.Object, _mockLogger.Object);
		}

		[Test]
		public async Task ObtenerPorIdPropiedad_ReturnsOk_WhenSuccess()
		{
			var traces = new List<PropertyTraceDto> { new PropertyTraceDto { IdProperty = "trace1" } };
			var response = ApiResponse<IEnumerable<PropertyTraceDto>>.Ok(traces);

			_mockService.Setup(s => s.ObtenerPorIdPropiedad("prop1")).ReturnsAsync(response);

			var result = await _controller.ObtenerPorIdPropiedad("prop1");

			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(200, okResult.StatusCode);

			var apiResponse = okResult.Value as ApiResponse<IEnumerable<PropertyTraceDto>>;
			Assert.IsTrue(apiResponse.Success);
			Assert.IsNotNull(apiResponse.Data);
		}

		[Test]
		public async Task ObtenerPorIdPropiedad_ReturnsNotFound_WhenNotFound()
		{
			var response = ApiResponse<IEnumerable<PropertyTraceDto>>.Fail("No existe");

			_mockService.Setup(s => s.ObtenerPorIdPropiedad("propNotFound")).ReturnsAsync(response);

			var result = await _controller.ObtenerPorIdPropiedad("propNotFound");

			var notFoundResult = result as NotFoundObjectResult;
			Assert.IsNotNull(notFoundResult);
			Assert.AreEqual(404, notFoundResult.StatusCode);
		}

		[Test]
		public async Task ObtenerPorIdPropiedad_ReturnsInternalServerError_OnException()
		{
			_mockService.Setup(s => s.ObtenerPorIdPropiedad(It.IsAny<string>())).ThrowsAsync(new Exception("Error"));

			var result = await _controller.ObtenerPorIdPropiedad("prop1");

			var objectResult = result as ObjectResult;
			Assert.IsNotNull(objectResult);
			Assert.AreEqual(500, objectResult.StatusCode);
		}

		[Test]
		public async Task Crear_ReturnsOk_WhenSuccess()
		{
			var request = new PropertyTraceRequest { PropertyId = "prop1", Name = "desc" };
			var response = ApiResponse<string>.Ok("trace1", "Creación exitosa");

			_mockService.Setup(s => s.Crear(request)).ReturnsAsync(response);

			var result = await _controller.Crear(request);

			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(200, okResult.StatusCode);

			var apiResponse = okResult.Value as ApiResponse<string>;
			Assert.IsTrue(apiResponse.Success);
			Assert.AreEqual("trace1", apiResponse.Data);
		}

		[Test]
		public async Task Crear_ReturnsBadRequest_WhenModelStateInvalid()
		{
			_controller.ModelState.AddModelError("Description", "Required");

			var result = await _controller.Crear(new PropertyTraceRequest());

			var badRequest = result as BadRequestObjectResult;
			Assert.IsNotNull(badRequest);
			Assert.AreEqual(400, badRequest.StatusCode);
		}

		[Test]
		public async Task Crear_ReturnsBadRequest_WhenServiceFails()
		{
			var request = new PropertyTraceRequest { PropertyId = "prop1", Name = "desc" };
			var response = ApiResponse<string>.Fail("Error");

			_mockService.Setup(s => s.Crear(request)).ReturnsAsync(response);

			var result = await _controller.Crear(request);

			var badRequest = result as BadRequestObjectResult;
			Assert.IsNotNull(badRequest);
			Assert.AreEqual(400, badRequest.StatusCode);
		}

		[Test]
		public async Task Crear_ReturnsInternalServerError_OnException()
		{
			var request = new PropertyTraceRequest { PropertyId = "prop1", Name = "desc" };
			_mockService.Setup(s => s.Crear(request)).ThrowsAsync(new Exception("Error"));

			var result = await _controller.Crear(request);

			var objectResult = result as ObjectResult;
			Assert.IsNotNull(objectResult);
			Assert.AreEqual(500, objectResult.StatusCode);
		}

		[Test]
		public async Task Eliminar_ReturnsOk_WhenSuccess()
		{
			var response = ApiResponse<string>.Ok("trace1", "Eliminación exitosa");

			_mockService.Setup(s => s.Eliminar("trace1")).ReturnsAsync(response);

			var result = await _controller.Eliminar("trace1");

			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Assert.AreEqual(200, okResult.StatusCode);
		}

		[Test]
		public async Task Eliminar_ReturnsBadRequest_WhenServiceFails()
		{
			var response = ApiResponse<string>.Fail("Error");

			_mockService.Setup(s => s.Eliminar("trace1")).ReturnsAsync(response);

			var result = await _controller.Eliminar("trace1");

			var badRequest = result as BadRequestObjectResult;
			Assert.IsNotNull(badRequest);
			Assert.AreEqual(400, badRequest.StatusCode);
		}

		[Test]
		public async Task Eliminar_ReturnsInternalServerError_OnException()
		{
			_mockService.Setup(s => s.Eliminar(It.IsAny<string>())).ThrowsAsync(new Exception("Error"));

			var result = await _controller.Eliminar("trace1");

			var objectResult = result as ObjectResult;
			Assert.IsNotNull(objectResult);
			Assert.AreEqual(500, objectResult.StatusCode);
		}
	}
}

using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Dto.Request;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Servicios.APLICACION_GESTION_PROPIEDADES.Servicios;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using Microsoft.Extensions.Logging;
using Moq;

[TestFixture]
public class PropertyTraceServicioTests
{
	private Mock<IPropertyTraceRepositorio> _mockTraceRepo;
	private Mock<IPropertyRespositorio> _mockPropiedadRepo;
	private Mock<ILogger<PropertyTraceServicio>> _mockLogger;
	private PropertyTraceServicio _service;

	[SetUp]
	public void Setup()
	{
		_mockTraceRepo = new Mock<IPropertyTraceRepositorio>();
		_mockPropiedadRepo = new Mock<IPropertyRespositorio>();
		_mockLogger = new Mock<ILogger<PropertyTraceServicio>>();
		_service = new PropertyTraceServicio(_mockTraceRepo.Object, _mockPropiedadRepo.Object, _mockLogger.Object);
	}

	[Test]
	public async Task Crear_ReturnsFail_WhenPropiedadNotFound()
	{
		// Arrange
		var request = new PropertyTraceRequest
		{
			PropertyId = "prop123",
			DateSale = DateTime.Now,
			Name = "Test Trace",
			Value = 1000,
			Tax = 100
		};
		_mockPropiedadRepo.Setup(r => r.ObtenerPorId(request.PropertyId)).ReturnsAsync((Property)null);

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
		var request = new PropertyTraceRequest
		{
			PropertyId = "prop123",
			DateSale = DateTime.Now,
			Name = "Test Trace",
			Value = 1000,
			Tax = 100
		};
		var propiedad = new Property { IdProperty = "prop123" };

		_mockPropiedadRepo.Setup(r => r.ObtenerPorId(request.PropertyId)).ReturnsAsync(propiedad);
		_mockTraceRepo.Setup(r => r.Crear(It.IsAny<PropertyTrace>())).Returns(Task.CompletedTask);

		// Act
		var result = await _service.Crear(request);

		// Assert
		Assert.IsTrue(result.Success);
		Assert.AreEqual("Traza registrada correctamente.", result.Message);

		_mockTraceRepo.Verify(r => r.Crear(It.Is<PropertyTrace>(t =>
			t.IdProperty == request.PropertyId &&
			t.Name == request.Name &&
			t.Value == request.Value &&
			t.Tax == request.Tax)), Times.Once);
	}

	[Test]
	public async Task Eliminar_ReturnsFail_WhenNotDeleted()
	{
		// Arrange
		string id = "trace123";
		_mockTraceRepo.Setup(r => r.Eliminar(id)).ReturnsAsync(false);

		// Act
		var result = await _service.Eliminar(id);

		// Assert
		Assert.IsFalse(result.Success);
		Assert.AreEqual("No se encontró la traza.", result.Message);
	}

	[Test]
	public async Task Eliminar_ReturnsOk_WhenDeleted()
	{
		// Arrange
		string id = "trace123";
		_mockTraceRepo.Setup(r => r.Eliminar(id)).ReturnsAsync(true);

		// Act
		var result = await _service.Eliminar(id);

		// Assert
		Assert.IsTrue(result.Success);
		Assert.AreEqual("Traza eliminada correctamente.", result.Message);
	}

	[Test]
	public async Task ObtenerPorIdPropiedad_ReturnsFail_WhenNoTraces()
	{
		// Arrange
		string idProperty = "prop123";
		_mockTraceRepo.Setup(r => r.ObtenerPorIdPropiedad(idProperty)).ReturnsAsync(new List<PropertyTrace>());

		// Act
		var result = await _service.ObtenerPorIdPropiedad(idProperty);

		// Assert
		Assert.IsFalse(result.Success);
		Assert.AreEqual("No se encontró la traza.", result.Message);
	}

	[Test]
	public async Task ObtenerPorIdPropiedad_ReturnsOk_WithTraceList()
	{
		// Arrange
		string idProperty = "prop123";
		var traces = new List<PropertyTrace>
		{
			new PropertyTrace { IdPropertyTrace = "t1", IdProperty = idProperty, Name = "Trace1", DateSale = DateTime.Now, Value = 1000, Tax = 100 },
			new PropertyTrace { IdPropertyTrace = "t2", IdProperty = idProperty, Name = "Trace2", DateSale = DateTime.Now, Value = 2000, Tax = 200 }
		};
		_mockTraceRepo.Setup(r => r.ObtenerPorIdPropiedad(idProperty)).ReturnsAsync(traces);

		// Act
		var result = await _service.ObtenerPorIdPropiedad(idProperty);

		// Assert
		Assert.IsTrue(result.Success);
		Assert.IsNotNull(result.Data);
		Assert.AreEqual(2, result.Data.Count());

		var first = result.Data.First();
		Assert.AreEqual("t1", first.IdPropertyTrace);
		Assert.AreEqual("Trace1", first.Name);
	}
}

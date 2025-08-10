using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using INFRAESTRUCTURA_GESTION_PROPIEDADES.Repositorio;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;

[TestFixture]
public class PropertyRepositorioTests
{
	private Mock<IMongoDbContext> _mockContext;
	private Mock<IMongoCollection<Property>> _mockCollection;
	private Mock<ILogger<PropertyRepositorio>> _mockLogger;
	private PropertyRepositorio _repository;

	[SetUp]
	public void Setup()
	{
		_mockContext = new Mock<IMongoDbContext>();
		_mockCollection = new Mock<IMongoCollection<Property>>();
		_mockLogger = new Mock<ILogger<PropertyRepositorio>>();

		// Configura que el contexto retorne la colección mockeada
		_mockContext.Setup(c => c.Properties).Returns(_mockCollection.Object);

		// Ahora crea repositorio con el contexto mock
		_repository = new PropertyRepositorio(_mockContext.Object, _mockLogger.Object);
	}


}

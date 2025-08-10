using APLICACION_GESTION_PROPIEDADES.Common.Constantes;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Servicios;
using DOMINIO_GESTION_PROPIEDADES.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;

namespace API_TEST
{
	[TestFixture]
	public class OwnerServicioTests
	{
		private Mock<IOwnerRepositorio> _mockRepo;
		private Mock<ILogger<OwnerServicio>> _mockLogger;
		private OwnerServicio _service;

		[SetUp]
		public void Setup()
		{
			_mockRepo = new Mock<IOwnerRepositorio>();
			_mockLogger = new Mock<ILogger<OwnerServicio>>();
			_service = new OwnerServicio(_mockRepo.Object, _mockLogger.Object);
		}

		[Test]
		public async Task ObtenerTodos_ReturnsOwners_WhenSuccess()
		{
			var owners = new List<Owner>
				{
					new Owner { Name = "Owner1", Address = "Addr1", Birthday = DateTime.Now, Photo = "photo1" }
				};

			_mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(owners);

			var result = await _service.ObtenerTodos();

			Assert.IsTrue(result.Success);
			Assert.IsNotNull(result.Data);

			// Convierte a lista antes de hacer el assert
			var listaOwners = result.Data.ToList();

			Assert.AreEqual(1, listaOwners.Count);
			Assert.AreEqual(MessageResponse.PropietariosObtenidos, result.Message);
		}


		[Test]
		public async Task ObtenerTodos_ReturnsFail_WhenException()
		{
			_mockRepo.Setup(r => r.ObtenerTodos()).ThrowsAsync(new Exception("error"));

			var result = await _service.ObtenerTodos();

			Assert.IsFalse(result.Success);
			Assert.AreEqual(MessageResponse.ErrorObtenerPropietarios, result.Message);

			_mockLogger.Verify(
					l => l.Log(
						LogLevel.Error,
						It.IsAny<EventId>(),
						It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(MessageResponse.ErrorObtenerPropietarios)),
						It.IsAny<Exception>(),
						It.IsAny<Func<It.IsAnyType, Exception, string>>()),
					Times.Once);
		}
		[Test]
		public async Task ObtenerPorId_ReturnsOwner_WhenFound()
		{
			var id = ObjectId.GenerateNewId();

			var owner = new Owner
			{
				Id = id,
				Name = "Owner1",
				Address = "Addr1",
				Birthday = DateTime.Now,
				Photo = "photo1"
			};

			_mockRepo.Setup(r => r.ObtenerPorId(id.ToString())).ReturnsAsync(owner);

			var result = await _service.ObtenerPorId(id.ToString());

			Assert.IsTrue(result.Success);
			Assert.IsNotNull(result.Data);
			Assert.AreEqual(id.ToString(), result.Data.Id);
			Assert.AreEqual(MessageResponse.PropietarioObtenido, result.Message);
		}



		[Test]
		public async Task ObtenerPorId_ReturnsFail_WhenNotFound()
		{
			_mockRepo.Setup(r => r.ObtenerPorId(It.IsAny<string>())).ReturnsAsync((Owner?)null);

			var result = await _service.ObtenerPorId("nonexistent");

			Assert.IsFalse(result.Success);
			Assert.AreEqual("Error al obtener el propietario.", result.Message);
		}

		[Test]
		public async Task ObtenerPorId_ReturnsFail_WhenException()
		{
			_mockRepo.Setup(r => r.ObtenerPorId(It.IsAny<string>())).ThrowsAsync(new Exception("error"));

			var result = await _service.ObtenerPorId("id");

			Assert.IsFalse(result.Success);
			Assert.AreEqual(MessageResponse.ErrorObtenerPropietario, result.Message);
			_mockLogger.Verify(
						l => l.Log(
							LogLevel.Error,
							It.IsAny<EventId>(),
							It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(MessageResponse.ErrorObtenerPropietario)),
							It.IsAny<Exception>(),
							It.IsAny<Func<It.IsAnyType, Exception, string>>()),
						Times.Once);
		}

		[Test]
		public async Task Crear_ReturnsSuccess_WhenCreated()
		{
			var ownerDto = new OwnerDto { Name = "Owner1" };
			_mockRepo.Setup(r => r.Crear(ownerDto));

			var result = await _service.Crear(ownerDto);

			Assert.IsTrue(result.Success);
			Assert.AreEqual(MessageResponse.PropietarioCreado, result.Message);
		}

		[Test]
		public async Task Crear_ReturnsFail_WhenException()
		{
			var ownerDto = new OwnerDto { Name = "Owner1" };
			_mockRepo.Setup(r => r.Crear(ownerDto)).ThrowsAsync(new Exception("error"));

			var result = await _service.Crear(ownerDto);

			Assert.IsFalse(result.Success);
			Assert.AreEqual(MessageResponse.ErrorCrearPropietario, result.Message);
			_mockLogger.Verify(
					l => l.Log(
						LogLevel.Error,
						It.IsAny<EventId>(),
						It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(MessageResponse.ErrorCrearPropietario)),
						It.IsAny<Exception>(),
						It.IsAny<Func<It.IsAnyType, Exception, string>>()),
					Times.Once);
		}

		[Test]
		public async Task Actualizar_ReturnsSuccess_WhenUpdated()
		{
			var id = Guid.NewGuid().ToString();
			var ownerDto = new OwnerDto { Name = "OwnerUpdated" };
			var existingOwner = new Owner { Name = "Owner1" };

			_mockRepo.Setup(r => r.ObtenerPorId(id)).ReturnsAsync(existingOwner);
			_mockRepo.Setup(r => r.Actualizar(id, ownerDto));

			var result = await _service.Actualizar(id, ownerDto);

			Assert.IsTrue(result.Success);
			Assert.AreEqual(MessageResponse.PropietarioActualizado, result.Message);
		}

		[Test]
		public async Task Actualizar_ReturnsFail_WhenOwnerNotFound()
		{
			_mockRepo.Setup(r => r.ObtenerPorId(It.IsAny<string>())).ReturnsAsync((Owner?)null);

			var result = await _service.Actualizar("id", new OwnerDto());

			Assert.IsFalse(result.Success);
			Assert.AreEqual(MessageResponse.PropietarioNoEncontrado, result.Message);
		}

		[Test]
		public async Task Actualizar_ReturnsFail_WhenException()
		{
			var id = Guid.NewGuid().ToString();
			var ownerDto = new OwnerDto { Name = "OwnerUpdated" };
			_mockRepo.Setup(r => r.ObtenerPorId(id)).ThrowsAsync(new Exception("error"));

			var result = await _service.Actualizar(id, ownerDto);

			Assert.IsFalse(result.Success);
			Assert.AreEqual(MessageResponse.ErrorActualizarPropietario, result.Message);
			_mockLogger.Verify(
				l => l.Log(
					LogLevel.Error,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(MessageResponse.ErrorActualizarPropietario)),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()),
				Times.Once);

		}

		[Test]
		public async Task Eliminar_ReturnsSuccess_WhenDeleted()
		{
			var id = Guid.NewGuid().ToString();
			var existingOwner = new Owner { Name = "Owner1" };

			_mockRepo.Setup(r => r.ObtenerPorId(id)).ReturnsAsync(existingOwner);
			_mockRepo.Setup(r => r.Eliminar(id));

			var result = await _service.Eliminar(id);

			Assert.IsTrue(result.Success);
			Assert.AreEqual(MessageResponse.PropietarioEliminado, result.Message);
		}

		[Test]
		public async Task Eliminar_ReturnsFail_WhenOwnerNotFound()
		{
			_mockRepo.Setup(r => r.ObtenerPorId(It.IsAny<string>())).ReturnsAsync((Owner?)null);

			var result = await _service.Eliminar("id");

			Assert.IsFalse(result.Success);
			Assert.AreEqual(MessageResponse.PropietarioNoEncontrado, result.Message);
		}
		[Test]
		public async Task Eliminar_ReturnsFail_WhenException()
		{
			var id = Guid.NewGuid().ToString();
			_mockRepo.Setup(r => r.ObtenerPorId(id)).ThrowsAsync(new Exception("error"));

			var result = await _service.Eliminar(id);

			Assert.IsFalse(result.Success);
			Assert.AreEqual(MessageResponse.ErrorEliminarPropietario, result.Message);

			_mockLogger.Verify(
				l => l.Log(
					LogLevel.Error,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(MessageResponse.ErrorEliminarPropietario)),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()),
				Times.Once);
		}

	}
}

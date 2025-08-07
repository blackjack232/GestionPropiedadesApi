using DOMINIO_GESTION_PROPIEDADES.Entities;
using INFRAESTRUCTURA_GESTION_PROPIEDADES.Contexto;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace API_GESTION_PROPIEDADES.Controllers.Propiedad
{
	[ApiController]
	[Route("api/[controller]")]
	public class TestMongoController : ControllerBase
	{
		private readonly MongoDbContext _context;

		public TestMongoController(MongoDbContext context)
		{
			_context = context;
		}

		[HttpGet("ping")]
		public async Task<IActionResult> Ping()
		{
			try
			{
				var total = await _context.Properties.CountDocumentsAsync(FilterDefinition<Property>.Empty);
				return Ok(new { message = "Conexión exitosa a MongoDB Atlas", total });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Error al conectar con MongoDB", error = ex.Message });
			}
		}
	}

}

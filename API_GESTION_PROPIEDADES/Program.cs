using APLICACION_GESTION_PROPIEDADES;
using APLICACION_GESTION_PROPIEDADES.Common.Interfaces.Repositorio;
using APLICACION_GESTION_PROPIEDADES.Dto;
using APLICACION_GESTION_PROPIEDADES.Interfaces.Repositorio;
using INFRAESTRUCTURA_GESTION_PROPIEDADES.Contexto;
using INFRAESTRUCTURA_GESTION_PROPIEDADES.Repositorio;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// CONFIGURAR SERILOG
Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDbSettings>(
	builder.Configuration.GetSection("MongoDbSettings"));

// Si usas MongoDbContext:
builder.Services.AddSingleton<MongoDbContext>();

// Registra el repositorio y servicio

builder.Services.AddScoped<IPropertyRespositorio, PropiedadRepositorio>();
builder.Services.AddScoped<IOwnerRepositorio, OwnerRepositorio>();
builder.Services.AddScoped<IPropertyImageRepositorio, PropertyImageRepositorio>();
builder.Services.AddScoped<IPropertyTraceRepositorio, PropertyTraceRepositorio>();

builder?.Services.AddApplication();

builder?.Services.AddCors(options =>
{
	options.AddPolicy("AllowLocalhost3000", policy =>
	{
		policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
			  .AllowAnyHeader()
			  .AllowAnyMethod()
			  .AllowCredentials();
	});
});
var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowLocalhost3000");

app.UseAuthorization();

app.MapControllers();


try
{
	Log.Information("Iniciando aplicación...");
	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
	Log.CloseAndFlush();
}

using Core.Framework.Aplication.Common.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// Configuración appsettings 
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Servicios
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration); // DB, repositorios
builder.Services.AddWebServices(builder.Configuration); // JWT, Swagger, CORS, etc.

// Build app
var app = builder.Build();

// Middleware / pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

// Inicializar datos Base en memoria
app.UseDataSeeder();

// Pipeline
app.UseRouting();
app.UseCors("_configCors");
app.UseMiddleware<ErrorHandlerMiddleware>();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoMetrics API V1");
    c.RoutePrefix = "swagger";

});

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Controllers y Logging

app.MapControllers();
app.UseSerilogRequestLogging();

// Run
app.Run();

using AutoMetricsService.Application.Common.Extensions;
using AutoMetricsService.Application.Common.Mappings;
using AutoMetricsService.Infrastructure.Data;
using Core.Framework.Aplication.Common.Security;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO.Compression;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuración de controllers con Newtonsoft.Json
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCaseAndSnakeCaseResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

        // Exception page para desarrollo
        services.AddDatabaseDeveloperPageExceptionFilter();

        // Scoped services
        services.AddHttpContextAccessor();

        // Health Checks
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddRazorPages();

        // Configuración de API
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        // Mapster
        services.AddMapsterServices();
        MappingConfig.RegisterMappings(TypeAdapterConfig.GlobalSettings);

        // Swagger / OpenAPI
        services.AddEndpointsApiExplorer();
        services.AddSwaggerServices();
        services.AddSwaggerGen();

        // Logging
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole();
        });

        // Compresión
        services.AddResponseCompression(options =>
        {
            var MimeTypes = new List<string>
            {
                "text/plain",
                "application/json"
            };
            options.EnableForHttps = true;
            options.MimeTypes = MimeTypes;
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("_configCors", policy =>
            {
                policy.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithExposedHeaders("X-Execution-Time-ms"); //Muestra tiempo de ejecución en milisegundos desde el middleware
            });
        });

        // MediatR: registra todos los handlers de la capa Application automáticamente
        var applicationAssembly = typeof(MappingConfig).Assembly; // cualquier tipo de la capa Application
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

        // JWT Authentication
        services.AddJwtAuthentication(configuration);

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.GetSection("JwtSettings").Bind(jwtSettings);

        services.AddSingleton(jwtSettings);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }


}


// Resolver que acepta camelCase y snake_case automáticamente
public class CamelCaseAndSnakeCaseResolver : DefaultContractResolver
{
    protected override string ResolvePropertyName(string propertyName)
    {
        // Mantiene camelCase al serializar
        return propertyName;
    }

    protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
    {
        var prop = base.CreateProperty(member, memberSerialization);
        // Permite deserializar snake_case automáticamente
        prop.PropertyName = prop.UnderlyingName;
        return prop;
    }
}

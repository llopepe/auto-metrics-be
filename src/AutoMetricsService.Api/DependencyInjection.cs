using AutoMetricsService.Application.Common.Extensions;
using AutoMetricsService.Application.Common.Mappings;
using AutoMetricsService.Infrastructure.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO.Compression;

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

        //Permiten exponer un endpoint que informa el estado del microservicio
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
                           .WithExposedHeaders("X-Execution-Time-ms");
            });
        });

        // MediatR: registra todos los handlers de la capa Application automáticamente
        var applicationAssembly = typeof(MappingConfig).Assembly; // cualquier tipo de la capa Application
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

        //JWT Settings
        //services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        //var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        //var key = Encoding.UTF8.GetBytes(jwtSettings!.Secret);

        //services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //})
        //    .AddJwtBearer(options =>
        //    {
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = true,

        //            ValidIssuer = jwtSettings.Issuer,
        //            ValidAudience = jwtSettings.Audience,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ClockSkew = TimeSpan.Zero // no da tolerancia en expiración
        //        };

        //        //Nuevo bloque para capturar errores y derivarlos al middleware
        //        options.Events = new JwtBearerEvents
        //        {
        //            OnAuthenticationFailed = context =>
        //            {
        //                throw new UnauthorizedAccessException("El token JWT es inválido o ha expirado.");
        //            },
        //            OnChallenge = context =>
        //            {
        //                context.HandleResponse();
        //                throw new UnauthorizedAccessException("Acceso denegado. Token no encontrado o inválido.");
        //            }
        //        };


        //    });


        //services.AddAuthorization();


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

using Core.Framework.Aplication.Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Common.Extensions
{
    public static class SwaggerExtensions
    {
        public static IApplicationBuilder UseSwaggerWithVersion(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", (context) => Task.Run(() => context.Response.Redirect($"swagger")));
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "AuthCore API");
            });

            return app;
        }

        public static void AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.EnableAnnotations();
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Net Auto Metrics",
                    Description = $"API Auto Metrics Build Date: {BuildInfoExtension.GetBuildDate()} <br/>"

                });
                swagger.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "duration",
                    Example = new OpenApiString("00:00:00")
                });

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                    }
                });


            });
        }
    }
}

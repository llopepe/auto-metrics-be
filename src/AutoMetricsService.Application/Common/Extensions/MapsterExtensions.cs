using AutoMetricsService.Application.Common.Mappings;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AutoMetricsService.Application.Common.Extensions
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class MapsterExtensions
    {

        public static void AddMapsterServices(this IServiceCollection services)
        {
            // Registro de Mapster
            var config = TypeAdapterConfig.GlobalSettings;
            MappingConfig.RegisterMappings(config); // Tu clase con los mapeos
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

        }
    }
}

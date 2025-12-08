using AutoMetricsService.Application.Sales.Dto;
using AutoMetricsService.Domain.Entities;
using Mapster;

namespace AutoMetricsService.Application.Common.Mappings
{
    public static class MappingConfig
    {
        public static void RegisterMappings(TypeAdapterConfig config)
        {

            TypeAdapterConfig<Sale, SaleDto>.NewConfig()
            .Map(dest => dest.CenterName, src => src.Center.Name)
            .Map(dest => dest.CarModel, src => src.Car.Name);
        }
    }
}

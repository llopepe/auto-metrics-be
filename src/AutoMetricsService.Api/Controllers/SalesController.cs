using AutoMetricsService.Application.Sales.CreateSale;
using AutoMetricsService.Application.Sales.Dto;
using AutoMetricsService.Application.Sales.Queries.GetPercentageGlobalWithPagination;
using AutoMetricsService.Application.Sales.Queries.GetSalesByCenterWithPagination;
using AutoMetricsService.Application.Sales.Queries.GetSaleWithPagination;
using AutoMetricsService.Application.Sales.Queries.GetTotalSalesVolume;
using Core.Framework.Aplication.Common.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AutoMetricsService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        [HttpPost("CreateSale")]
        [SwaggerOperation(
                Summary = "Inserta una venta", 
                Description = "Permite registrar los datos de ventas, calcular el precio total según el CarId y, en caso de que el vehículo tenga impuestos asociados, sumarlos automáticamente al total.")]
        public Task<ResultResponse<int>> CreateSystem(ISender sender, CreateSaleCommand command)
        {
            // llama al comando
            return sender.Send(command);
        }

        [HttpGet("GetSaleWithPagination")]
        [SwaggerOperation(
                Summary = "Obtener listado paginado de ventas de todos los centros", 
                Description = "Listado de ventas Paginados")]
        public Task<PaginatedList<SaleDto>> GetAllWithPagination(ISender sender, [FromQuery] GetSaleWithPaginationQuery query,
                                                                                 CancellationToken cancellationToken)
        {
            // llama al comando
            return sender.Send(query, cancellationToken);
        }

        [HttpGet("GetTotalSalesVolume")]
        [SwaggerOperation(
                Summary = "Obtener el volumen de ventas total", 
                Description = "Obtiene el total de todos los centros")]
        public Task<ResultResponse<TotalSalesVolumeDto>> GetTotalSalesVolume(ISender sender, CancellationToken cancellationToken)
        {
            // llama al comando
            return sender.Send(new GetTotalSalesVolumeQuery(), cancellationToken);
        }

        [HttpGet("GetSalesByCenterWithPagination")]
        [SwaggerOperation(
            Summary ="Obtener volumen de ventas por centro", 
            Description ="Obtiene el total por centro paginados, tiene un filtro opcional que permite filtrar los datos por CentroId")]
        public Task<PaginatedList<SalesVolumeCenterDto>> GetSalesByCenterWithPagination(ISender sender, [FromQuery] GetSalesByCenterWithPaginationQuery query,
                                                                                 CancellationToken cancellationToken)
        {
            // llama al comando
            return sender.Send(query, cancellationToken);
        }

        [HttpGet("GetPercentageGlobalWithPagination")]
        [SwaggerOperation(
       Summary = "Obtener el porcentaje de unidades de cada modelo vendido en cada centro sobre el total de ventas.",
       Description = "Obtiene los datos paginados")]
        public Task<PaginatedList<PercentageGlobalDto>> GetPercentageGlobalWithPagination(ISender sender, [FromQuery] GetPercentageGlobalWithPaginationQuery query,
                                                                            CancellationToken cancellationToken)
        {
            // llama al comando
            return sender.Send(query, cancellationToken);
        }
    }
}

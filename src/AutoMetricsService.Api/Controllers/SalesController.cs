using AutoMetricsService.Application.Sales.Queries.GetSaleWithPagination;
using AutoMetricsService.Application.Sales.CreateSale;
using AutoMetricsService.Application.Sales.Dto;
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
        [HttpGet("GetSaleWithPagination")]
        [SwaggerOperation("Obtener todas las ventas", "Listado de ventas Paginados")]
        public Task<PaginatedList<SaleDto>> GetAllWithPagination(ISender sender, [FromQuery] GetSaleWithPaginationQuery query)
        {
            // llama al comando
            return sender.Send(query);
        }



        [HttpPost("CreateSale")]
        [SwaggerOperation("Crear un venta", "Datos del ventas")]
        public Task<ResultResponse<int>> CreateSystem(ISender sender, CreateSaleCommand command)
        {
            return sender.Send(command);
        }
    }
}

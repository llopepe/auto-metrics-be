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
        [HttpGet("GetSystemWithPagination")]
        [SwaggerOperation("Obtener todos los sistemas", "Listado de sistemas Paginados")]
        public Task<PaginatedList<SaleDto>> GetAllWithPagination(ISender sender, [FromQuery] GetSaleWithPaginationQuery query)
        {
            return sender.Send(query);
        }



        [HttpPost("CreateSale")]
        [SwaggerOperation("Crear un sistema", "Datos del sistema")]
        public Task<ResultResponse<int>> CreateSystem(ISender sender, CreateSaleCommand command)
        {
            return sender.Send(command);
        }
    }
}

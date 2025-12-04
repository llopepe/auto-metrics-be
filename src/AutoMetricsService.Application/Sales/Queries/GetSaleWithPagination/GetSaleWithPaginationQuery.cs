using AutoMetricsService.Application.Common.Extensions;
using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.CreateSale;
using AutoMetricsService.Application.Sales.Dto;
using AutoMetricsService.Domain.Entities;
using Core.Framework.Aplication.Common.Wrappers;
using Mapster;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Sales.Queries.GetSaleWithPagination
{
    public record GetSaleWithPaginationQuery : IRequest<PaginatedList<SaleDto>>
    {
        public string Search { get; init; } = string.Empty;
        public string SortOrder { get; init; } = string.Empty;
        public string SortDirection { get; init; } = string.Empty;
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }

    public class GetSystemsWithPaginationQueryHandler : IRequestHandler<GetSaleWithPaginationQuery, PaginatedList<SaleDto>>
    {
        private readonly ILogger<GetSaleWithPaginationQuery> _logger;
        private readonly ISaleRepository _saleRepository;

        public GetSystemsWithPaginationQueryHandler(ISaleRepository saleRepository, ILogger<GetSaleWithPaginationQuery> logger)
        {
            _saleRepository = saleRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<SaleDto>> Handle(GetSaleWithPaginationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ejecutando {Command}", request.GetType().Name);

            //Obtengo los datos Paginados
            var result = await _saleRepository.GetAllPaginatedSearch(request.PageNumber, 
                                                                    request.PageSize, 
                                                                    request.Search, 
                                                                    request.SortOrder, 
                                                                    request.SortDirection);

            // Convierto los datos
            return result.AdaptPaginated<Sale, SaleDto>();
        }
    }
}

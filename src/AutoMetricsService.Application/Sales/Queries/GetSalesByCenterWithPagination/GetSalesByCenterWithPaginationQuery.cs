using AutoMetricsService.Application.Common.Extensions;
using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.CreateSale;
using AutoMetricsService.Application.Sales.Dto;
using AutoMetricsService.Domain.Entities;
using AutoMetricsService.Domain.EntitiesCustom;
using Core.Framework.Aplication.Common.Wrappers;
using Mapster;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Sales.Queries.GetSalesByCenterWithPagination
{
    public record GetSalesByCenterWithPaginationQuery : IRequest<PaginatedList<SalesVolumeCenterDto>>
    {
        public int? CenterId { get; set; }
        public string SortOrder { get; init; } = string.Empty;
        public string SortDirection { get; init; } = string.Empty;
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }

    public class GetSalesByCenterWithPaginationQueryHandler : IRequestHandler<GetSalesByCenterWithPaginationQuery, PaginatedList<SalesVolumeCenterDto>>
    {
        private readonly ILogger<GetSalesByCenterWithPaginationQuery> _logger;
        private readonly ISaleRepository _saleRepository;

        public GetSalesByCenterWithPaginationQueryHandler(ISaleRepository saleRepository, ILogger<GetSalesByCenterWithPaginationQuery> logger)
        {
            _saleRepository = saleRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<SalesVolumeCenterDto>> Handle(GetSalesByCenterWithPaginationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ejecutando {Command}", request.GetType().Name);

            //Obtengo los datos Paginados
            var result = await _saleRepository.GetSalesByCenterPaginatedAsync(request.CenterId, request.PageNumber, 
                                                                            request.PageSize, request.SortOrder, 
                                                                            request.SortDirection, cancellationToken);

            // Convierto los datos
            return result.AdaptPaginated<SalesVolumeCenterCustom, SalesVolumeCenterDto>();

        }
    }
}

using AutoMetricsService.Application.Common.Extensions;
using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.Dto;
using AutoMetricsService.Application.Sales.Queries.GetSalesByCenterWithPagination;
using AutoMetricsService.Domain.EntitiesCustom;
using Core.Framework.Aplication.Common.Wrappers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Sales.Queries.GetPercentageGlobalWithPagination
{
    public record GetPercentageGlobalWithPaginationQuery : IRequest<PaginatedList<PercentageGlobalDto>>
    {
        public int? CenterId { get; set; }
        public string SortOrder { get; init; } = string.Empty;
        public string SortDirection { get; init; } = string.Empty;
        public int PageNumber { get; init; }
        public int PageSize { get; init; }

    }

    public class GetPercentageGlobalWithPaginationQueryHandler : IRequestHandler<GetPercentageGlobalWithPaginationQuery, PaginatedList<PercentageGlobalDto>>
    {

        private readonly ILogger<GetPercentageGlobalWithPaginationQuery> _logger;
        private readonly ISaleRepository _saleRepository;


        public GetPercentageGlobalWithPaginationQueryHandler(ISaleRepository saleRepository, ILogger<GetPercentageGlobalWithPaginationQuery> logger)
        {
            _saleRepository = saleRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<PercentageGlobalDto>> Handle(GetPercentageGlobalWithPaginationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ejecutando {Command}", request.GetType().Name);

            //Obtengo los datos Paginados
            var result = await _saleRepository.GetPercentageGlobalPaginatedAsync(request.PageNumber,
                                                                            request.PageSize, request.SortOrder,
                                                                            request.SortDirection, cancellationToken);

            // Convierto los datos
            return result.AdaptPaginated<PercentageGlobalCustom, PercentageGlobalDto>();

        }

    }
}

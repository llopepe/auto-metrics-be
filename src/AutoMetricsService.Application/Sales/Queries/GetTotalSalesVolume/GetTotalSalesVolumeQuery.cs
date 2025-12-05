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

namespace AutoMetricsService.Application.Sales.Queries.GetTotalSalesVolume
{
    public record GetTotalSalesVolumeQuery : IRequest<ResultResponse<TotalSalesVolumeDto>>
    {
    }

    public class GetTotalSalesVolumeQueryHandler : IRequestHandler<GetTotalSalesVolumeQuery, ResultResponse<TotalSalesVolumeDto>>
    {
        private readonly ILogger<GetTotalSalesVolumeQuery> _logger;
        private readonly ISaleRepository _saleRepository;

        public GetTotalSalesVolumeQueryHandler(ISaleRepository saleRepository, ILogger<GetTotalSalesVolumeQuery> logger)
        {
            _saleRepository = saleRepository;
            _logger = logger;
        }

        public async Task<ResultResponse<TotalSalesVolumeDto>> Handle(GetTotalSalesVolumeQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ejecutando {Command}", request.GetType().Name);

            return new TotalSalesVolumeDto
            {
                TotalVolume = await _saleRepository.GetTotalSalesVolumeAsync(cancellationToken)
            };
        }
    }
}

using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.Dto;
using Core.Framework.Aplication.Common.Wrappers;
using Microsoft.Extensions.Logging;
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

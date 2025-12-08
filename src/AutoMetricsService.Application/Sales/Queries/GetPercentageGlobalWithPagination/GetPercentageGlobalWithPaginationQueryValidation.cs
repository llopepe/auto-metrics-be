using AutoMetricsService.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Sales.Queries.GetSalesByCenterWithPagination
{
    public class GetPercentageGlobalWithPaginationQueryValidation : AbstractValidator<GetSalesByCenterWithPaginationQuery>
    {
        private readonly ICenterRepository _centerRepository;

        // Validaciones de datos de entreda de la query
        public GetPercentageGlobalWithPaginationQueryValidation(ICenterRepository centerRepository)
        {
            _centerRepository = centerRepository;

            RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber debe ser mayor igual a 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize debe ser mayor igual a 1.");
        }


        private async Task<bool> CenterExists(int centerId, CancellationToken ct)
        {
            return await _centerRepository.ExistsAsync(centerId, ct);
        }
    }
}

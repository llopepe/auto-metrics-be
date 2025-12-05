using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.Queries.GetSaleWithPagination;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            RuleFor(x => x.CenterId)
            .GreaterThanOrEqualTo(1)
            .WithMessage("CenterId debe ser mayor o igual a 1.")
            .MustAsync(async (model, centerId, ct) =>
                         await CenterExists(centerId!.Value, ct))
            .WithMessage("El CenterId especificado no existe.")
            .When(x => x.CenterId != null);

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

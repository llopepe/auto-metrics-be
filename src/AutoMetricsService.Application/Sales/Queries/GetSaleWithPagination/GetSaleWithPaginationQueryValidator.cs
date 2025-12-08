namespace AutoMetricsService.Application.Sales.Queries.GetSaleWithPagination
{
    public class GetSaleWithPaginationQueryValidator : AbstractValidator<GetSaleWithPaginationQuery>
    {

        public GetSaleWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
               .GreaterThanOrEqualTo(1).WithMessage("PageNumber debe ser mayor igual a 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize debe ser mayor igual a 1.");
        }

    }

}

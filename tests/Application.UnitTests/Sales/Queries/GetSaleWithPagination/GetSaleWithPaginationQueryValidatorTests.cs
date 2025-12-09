using AutoMetricsService.Application.Sales.Queries.GetSaleWithPagination;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Sales.Queries.GetSaleWithPagination
{
    public class GetSaleWithPaginationQueryValidatorTests
    {
        private readonly GetSaleWithPaginationQueryValidator _validator;

        public GetSaleWithPaginationQueryValidatorTests()
        {
            _validator = new GetSaleWithPaginationQueryValidator();
        }

        [Test]
        public void Should_Have_Error_When_PageNumber_Is_Less_Than_1()
        {
            var model = new GetSaleWithPaginationQuery { PageNumber = 0, PageSize = 10 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
        }

        [Test]
        public void Should_Have_Error_When_PageSize_Is_Less_Than_1()
        {
            var model = new GetSaleWithPaginationQuery { PageNumber = 1, PageSize = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }

        [Test]
        public void Should_Not_Have_Error_When_Values_Are_Valid()
        {
            var model = new GetSaleWithPaginationQuery { PageNumber = 1, PageSize = 10 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

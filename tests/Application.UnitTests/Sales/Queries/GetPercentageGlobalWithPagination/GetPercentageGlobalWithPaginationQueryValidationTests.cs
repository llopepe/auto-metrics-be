using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.Queries.GetSalesByCenterWithPagination;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Sales.Queries.GetPercentageGlobalWithPagination
{
    [TestFixture]
    public class GetPercentageGlobalWithPaginationQueryValidationTests
    {
        private Mock<ICenterRepository> _centerRepoMock = null!;
        private GetPercentageGlobalWithPaginationQueryValidation _validator = null!;

        [SetUp]
        public void Setup()
        {
            _centerRepoMock = new Mock<ICenterRepository>();
            _validator = new GetPercentageGlobalWithPaginationQueryValidation(_centerRepoMock.Object);
        }

        [Test]
        public async Task Validator_ShouldHaveError_WhenPageNumberIsLessThanOne()
        {
            var query = new GetSalesByCenterWithPaginationQuery { PageNumber = 0, PageSize = 10 };
            var result = await _validator.TestValidateAsync(query);

            result.ShouldHaveValidationErrorFor(q => q.PageNumber)
                  .WithErrorMessage("PageNumber debe ser mayor igual a 1.");
        }

        [Test]
        public async Task Validator_ShouldHaveError_WhenPageSizeIsLessThanOne()
        {
            var query = new GetSalesByCenterWithPaginationQuery { PageNumber = 1, PageSize = 0 };
            var result = await _validator.TestValidateAsync(query);

            result.ShouldHaveValidationErrorFor(q => q.PageSize)
                  .WithErrorMessage("PageSize debe ser mayor igual a 1.");
        }

        [Test]
        public async Task Validator_ShouldNotHaveError_WhenPageNumberAndSizeAreValid()
        {
            var query = new GetSalesByCenterWithPaginationQuery { PageNumber = 1, PageSize = 10 };
            var result = await _validator.TestValidateAsync(query);

            result.ShouldNotHaveValidationErrorFor(q => q.PageNumber);
            result.ShouldNotHaveValidationErrorFor(q => q.PageSize);
        }
    }
}

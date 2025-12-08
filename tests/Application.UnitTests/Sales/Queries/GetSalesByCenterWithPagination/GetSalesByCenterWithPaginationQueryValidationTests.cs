using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.Queries.GetSalesByCenterWithPagination;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Sales.Queries.GetSalesByCenterWithPagination
{
    [TestFixture]
    public class GetSalesByCenterWithPaginationQueryValidationTests
    {
        private Mock<ICenterRepository> _centerRepositoryMock = null!;
        private GetSalesByCenterWithPaginationQueryValidation _validator = null!;

        [SetUp]
        public void Setup()
        {
            _centerRepositoryMock = new Mock<ICenterRepository>();
            _validator = new GetSalesByCenterWithPaginationQueryValidation(_centerRepositoryMock.Object);
        }

        [Test]
        public async Task Should_HaveError_When_PageNumberIsZero()
        {
            var query = new GetSalesByCenterWithPaginationQuery { PageNumber = 0, PageSize = 10 };
            var result = await _validator.TestValidateAsync(query);
            result.ShouldHaveValidationErrorFor(q => q.PageNumber)
                  .WithErrorMessage("PageNumber debe ser mayor igual a 1.");
        }

        [Test]
        public async Task Should_HaveError_When_PageSizeIsZero()
        {
            var query = new GetSalesByCenterWithPaginationQuery { PageNumber = 1, PageSize = 0 };
            var result = await _validator.TestValidateAsync(query);
            result.ShouldHaveValidationErrorFor(q => q.PageSize)
                  .WithErrorMessage("PageSize debe ser mayor igual a 1.");
        }

        [Test]
        public async Task Should_HaveError_When_CenterIdDoesNotExist()
        {
            var query = new GetSalesByCenterWithPaginationQuery { CenterId = 999, PageNumber = 1, PageSize = 10 };

            _centerRepositoryMock.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(false);

            var result = await _validator.TestValidateAsync(query);
            result.ShouldHaveValidationErrorFor(q => q.CenterId)
                  .WithErrorMessage("El CenterId especificado no existe.");
        }

        [Test]
        public async Task Should_NotHaveError_When_ValidRequest()
        {
            var query = new GetSalesByCenterWithPaginationQuery { CenterId = 1, PageNumber = 1, PageSize = 10 };

            _centerRepositoryMock.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(true);

            var result = await _validator.TestValidateAsync(query);
            result.ShouldNotHaveValidationErrorFor(q => q.PageNumber);
            result.ShouldNotHaveValidationErrorFor(q => q.PageSize);
            result.ShouldNotHaveValidationErrorFor(q => q.CenterId);
        }
    }
}

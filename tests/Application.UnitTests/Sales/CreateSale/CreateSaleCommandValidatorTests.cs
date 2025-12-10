using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.CreateSale;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Sales.CreateSale
{
    [TestFixture]
    public class CreateSaleCommandValidatorTests
    {
        private Mock<ICarRepository> _carRepoMock = null!;
        private Mock<ICenterRepository> _centerRepoMock = null!;
        private CreateSaleCommandValidator _validator = null!;

        [SetUp]
        public void Setup()
        {
            _carRepoMock = new Mock<ICarRepository>();
            _centerRepoMock = new Mock<ICenterRepository>();
            _validator = new CreateSaleCommandValidator(_carRepoMock.Object, _centerRepoMock.Object);
        }

        [Test]
        public async Task Should_HaveNoValidationErrors_WhenCommandIsValid()
        {
            var command = new CreateSaleCommand { CenterId = 1, CarId = 100, Units = 5 };

            _centerRepoMock.Setup(x => x.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _carRepoMock.Setup(x => x.ExistsAsync(100, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public async Task Should_HaveValidationError_WhenCenterDoesNotExist()
        {
            var command = new CreateSaleCommand { CenterId = 999, CarId = 100, Units = 5 };

            _centerRepoMock.Setup(x => x.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _carRepoMock.Setup(x => x.ExistsAsync(100, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.CenterId)
                  .WithErrorMessage("El CenterId especificado no existe en la base de datos.");
        }

        [Test]
        public async Task Should_HaveValidationError_WhenCarDoesNotExist()
        {
            var command = new CreateSaleCommand { CenterId = 1, CarId = 999, Units = 5 };

            _centerRepoMock.Setup(x => x.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _carRepoMock.Setup(x => x.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.CarId)
                  .WithErrorMessage("El CarId especificado no existe en la base de datos.");
        }

        [Test]
        public async Task Should_HaveValidationError_WhenUnitsIsZeroOrNegative()
        {
            var command = new CreateSaleCommand { CenterId = 1, CarId = 100, Units = 0 };

            _centerRepoMock.Setup(x => x.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _carRepoMock.Setup(x => x.ExistsAsync(100, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Units)
                  .WithErrorMessage("Units debe ser mayor a 0.");
        }
    }
}

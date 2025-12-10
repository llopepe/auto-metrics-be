using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.CreateSale;
using AutoMetricsService.Domain.Entities;
using Core.Framework.Aplication.Interfaces.Data;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Sales.CreateSale
{
    [TestFixture]
    public class CreateSaleCommandTests
    {
        private Mock<ISaleRepository> _saleRepoMock = null!;
        private Mock<ICarRepository> _carRepoMock = null!;
        private Mock<ICarTaxRepository> _carTaxRepoMock = null!;
        private Mock<IUnitOfWork> _unitOfWorkMock = null!;
        private Mock<ILogger<CreateSaleCommandHandler>> _loggerMock = null!;
        private CreateSaleCommandHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _saleRepoMock = new Mock<ISaleRepository>();
            _carRepoMock = new Mock<ICarRepository>();
            _carTaxRepoMock = new Mock<ICarTaxRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreateSaleCommandHandler>>();

            _handler = new CreateSaleCommandHandler(
                _saleRepoMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _carRepoMock.Object,
                _carTaxRepoMock.Object
            );
        }

        [Test]
        public async Task Handle_ShouldCreateSaleAndAddDomainEvent_WhenValidRequest()
        {

            var command = new CreateSaleCommand
            {
                CenterId = 1,
                CarId = 100,
                Units = 2
            };

            var car = new Car { Id = 100, Price = 500m };

            _carRepoMock.Setup(x => x.GetByIdAsync(100)).ReturnsAsync(car);
            _carTaxRepoMock.Setup(x => x.GetTaxesByCarIdAsync(100)).ReturnsAsync(new List<CarTax>());

            _saleRepoMock.Setup(x => x.AddAsync(It.IsAny<Sale>())).ReturnsAsync((Sale s) =>
            {
                s.Id = 999;
                return s;
            });

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(999, result.Data);
            _saleRepoMock.Verify(x => x.AddAsync(It.IsAny<Sale>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);

            _saleRepoMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Handle_ShouldThrowException_WhenUnitsAreZero()
        {
            var command = new CreateSaleCommand
            {
                CenterId = 1,
                CarId = 100,
                Units = 0
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenCarNotFound()
        {
            var command = new CreateSaleCommand
            {
                CenterId = 1,
                CarId = 100,
                Units = 2
            };

            _carRepoMock.Setup(x => x.GetByIdAsync(100)).ReturnsAsync((Car)null);

            Assert.ThrowsAsync<Exception>(async () =>
                await _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_ShouldCalculateTotalWithTaxes()
        {
            var command = new CreateSaleCommand
            {
                CenterId = 1,
                CarId = 100,
                Units = 2
            };

            var car = new Car { Id = 100, Price = 100m };

            var taxes = new List<CarTax>
            {
                new CarTax { Id = 1, CarId = 100, Name = "Tax1", Percentage = 10m },
                new CarTax { Id = 2, CarId = 100, Name = "Tax2", Percentage = 5m }
            };

            _carRepoMock.Setup(x => x.GetByIdAsync(100)).ReturnsAsync(car);
            _carTaxRepoMock.Setup(x => x.GetTaxesByCarIdAsync(100)).ReturnsAsync(taxes);
            _saleRepoMock.Setup(x => x.AddAsync(It.IsAny<Sale>())).ReturnsAsync((Sale s) => { s.Id = 1; return s; });
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = _handler.Handle(command, CancellationToken.None).Result;

            // Total esperado: base 100*2=200, impuestos: 200*0.10=20 + 200*0.05=10 -> total=230
            _saleRepoMock.Verify(x => x.AddAsync(It.Is<Sale>(s => s.Total == 230m)), Times.Once);
        }


    }
}

using AutoMetricsService.Api.Controllers;
using AutoMetricsService.Application.Sales.CreateSale;
using AutoMetricsService.Application.Sales.Dto;
using AutoMetricsService.Application.Sales.Queries.GetPercentageGlobalWithPagination;
using AutoMetricsService.Application.Sales.Queries.GetSalesByCenterWithPagination;
using AutoMetricsService.Application.Sales.Queries.GetSaleWithPagination;
using AutoMetricsService.Application.Sales.Queries.GetTotalSalesVolume;
using Core.Framework.Aplication.Common.Wrappers;
using MediatR;
using Moq;

namespace Application.Functional.UnitTests.Controllers
{
    public class SalesControllerTests
    {
        private readonly Mock<ISender> _mockSender;
        private readonly SalesController _controller;

        public SalesControllerTests()
        {
            _mockSender = new Mock<ISender>();
            _controller = new SalesController();
        }

        [Test]
        public async Task CreateSystem_ReturnsResultResponse_WithId()
        {
            var command = new CreateSaleCommand
            {
                CarId = 1,
                CenterId = 1,
                Units = 2,
            };

            var expectedResult = ResultResponse<int>.Ok(42);

            _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expectedResult);

            var result = await _controller.CreateSystem(_mockSender.Object, command);

            Assert.True(result.Success);
            Assert.AreEqual(42, result.Data);
        }

        [Test]
        public async Task GetAllWithPagination_ReturnsPaginatedList()
        {
            var query = new GetSaleWithPaginationQuery { PageNumber = 1, PageSize = 10 };
            var expected = new PaginatedList<SaleDto>(new List<SaleDto>(), 0, 1, 10);

            _mockSender.Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expected);

            var result = await _controller.GetAllWithPagination(_mockSender.Object, query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.That(result, Is.TypeOf<PaginatedList<SaleDto>>());
        }

        [Test]
        public async Task GetTotalSalesVolume_ReturnsResultResponse()
        {

            var expected = ResultResponse<TotalSalesVolumeDto>.Ok(new TotalSalesVolumeDto { TotalVolume = 1000 });

            _mockSender.Setup(s => s.Send(It.IsAny<GetTotalSalesVolumeQuery>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expected);

            var result = await _controller.GetTotalSalesVolume(_mockSender.Object, CancellationToken.None);

            Assert.True(result.Success);
            Assert.AreEqual(1000, result.Data.TotalVolume);
        }

        [Test]
        public async Task GetSalesByCenterWithPagination_ReturnsPaginatedList()
        {

            var query = new GetSalesByCenterWithPaginationQuery { PageNumber = 1, PageSize = 5 };
            var expected = new PaginatedList<SalesVolumeCenterDto>(new List<SalesVolumeCenterDto>(), 0, 1, 5);

            _mockSender.Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expected);

            var result = await _controller.GetSalesByCenterWithPagination(_mockSender.Object, query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.That(result, Is.TypeOf<PaginatedList<SalesVolumeCenterDto>>());
        }

        [Test]
        public async Task GetPercentageGlobalWithPagination_ReturnsPaginatedList()
        {

            var query = new GetPercentageGlobalWithPaginationQuery { PageNumber = 1, PageSize = 5 };
            var expected = new PaginatedList<PercentageGlobalDto>(new List<PercentageGlobalDto>(), 0, 1, 5);

            _mockSender.Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expected);

            var result = await _controller.GetPercentageGlobalWithPagination(_mockSender.Object, query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.That(result, Is.TypeOf<PaginatedList<PercentageGlobalDto>>());
        }
    }
}

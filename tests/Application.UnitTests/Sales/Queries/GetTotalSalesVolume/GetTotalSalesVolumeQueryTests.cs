using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.Dto;
using AutoMetricsService.Application.Sales.Queries.GetTotalSalesVolume;
using Core.Framework.Aplication.Common.Wrappers;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Sales.Queries.GetTotalSalesVolume
{
    public class GetTotalSalesVolumeQueryTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<ILogger<GetTotalSalesVolumeQuery>> _loggerMock;

        public GetTotalSalesVolumeQueryTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _loggerMock = new Mock<ILogger<GetTotalSalesVolumeQuery>>();
        }

        [Test]
        public async Task Handle_ShouldReturnTotalSalesVolumeDto_WithExpectedValue()
        {
            var expectedValue = 1500m;

            _saleRepositoryMock
                .Setup(x => x.GetTotalSalesVolumeAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedValue);

            var handler = new GetTotalSalesVolumeQueryHandler(
                _saleRepositoryMock.Object,
                _loggerMock.Object
            );

            var query = new GetTotalSalesVolumeQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.That(result, Is.TypeOf<ResultResponse<TotalSalesVolumeDto>>());
            Assert.True(result.Success);
            Assert.AreEqual(expectedValue, result.Data.TotalVolume);

            _saleRepositoryMock.Verify(
                x => x.GetTotalSalesVolumeAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Test]
        public async Task Handle_ShouldReturnZero_WhenRepositoryReturnsZero()
        {
            _saleRepositoryMock
                .Setup(x => x.GetTotalSalesVolumeAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m);

            var handler = new GetTotalSalesVolumeQueryHandler(
                _saleRepositoryMock.Object,
                _loggerMock.Object
            );

            var query = new GetTotalSalesVolumeQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.AreEqual(0m, result.Data.TotalVolume);
            Assert.True(result.Success);

            _saleRepositoryMock.Verify(
                x => x.GetTotalSalesVolumeAsync(It.IsAny<CancellationToken>()),
                Times.AtLeastOnce()
            );
        }
    }
}

using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.Queries.GetSalesByCenterWithPagination;
using AutoMetricsService.Domain.EntitiesCustom;
using Core.Framework.Aplication.Common.Wrappers;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Sales.Queries.GetSalesByCenterWithPagination
{
    [TestFixture]
    public class GetSalesByCenterWithPaginationQueryHandlerTests
    {
        private Mock<ISaleRepository> _saleRepositoryMock = null!;
        private Mock<ILogger<GetSalesByCenterWithPaginationQuery>> _loggerMock = null!;
        private GetSalesByCenterWithPaginationQueryHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _loggerMock = new Mock<ILogger<GetSalesByCenterWithPaginationQuery>>();
            _handler = new GetSalesByCenterWithPaginationQueryHandler(_saleRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnPaginatedSales_WhenValidRequest()
        {

            var query = new GetSalesByCenterWithPaginationQuery
            {
                CenterId = null,
                PageNumber = 1,
                PageSize = 10,
                SortOrder = "CenterId",
                SortDirection = "asc"
            };

            var repoData = new List<SalesVolumeCenterCustom>
            {
                new SalesVolumeCenterCustom { CenterId = 1, CenterName = "Centro1", SalesVolume = 100 },
                new SalesVolumeCenterCustom { CenterId = 2, CenterName = "Centro2", SalesVolume = 200 },
            };

            var mockData = repoData.AsQueryable().BuildMock();

            var paginatedList = await PaginatedList<SalesVolumeCenterCustom>.CreateAsync(mockData, query.PageNumber, query.PageSize);

            _saleRepositoryMock.Setup(r => r.GetSalesByCenterPaginatedAsync(
                query.CenterId,
                query.PageNumber,
                query.PageSize,
                query.SortOrder,
                query.SortDirection,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedList);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsNotNull(result);
            var list = result.Items.ToList();
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list[0].CenterId);
            Assert.AreEqual("Centro1", list[0].CenterName);
            Assert.AreEqual(100, list[0].SalesVolume);

            _saleRepositoryMock.Verify(r => r.GetSalesByCenterPaginatedAsync(
                query.CenterId,
                query.PageNumber,
                query.PageSize,
                query.SortOrder,
                query.SortDirection,
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

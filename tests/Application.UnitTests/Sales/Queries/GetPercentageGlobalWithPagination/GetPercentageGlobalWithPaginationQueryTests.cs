using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.Queries.GetPercentageGlobalWithPagination;
using AutoMetricsService.Domain.EntitiesCustom;
using Core.Framework.Aplication.Common.Wrappers;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Sales.Queries.GetPercentageGlobalWithPagination
{
    [TestFixture]
    public class GetPercentageGlobalWithPaginationQueryTests
    {
        private Mock<ISaleRepository> _saleRepoMock = null!;
        private Mock<ILogger<GetPercentageGlobalWithPaginationQuery>> _loggerMock = null!;
        private GetPercentageGlobalWithPaginationQueryHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _saleRepoMock = new Mock<ISaleRepository>();
            _loggerMock = new Mock<ILogger<GetPercentageGlobalWithPaginationQuery>>();
            _handler = new GetPercentageGlobalWithPaginationQueryHandler(_saleRepoMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnPaginatedPercentageGlobalDto()
        {
            var query = new GetPercentageGlobalWithPaginationQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SortOrder = "CenterId",
                SortDirection = "asc"
            };

            var data = new List<PercentageGlobalCustom>
            {
                new() { CenterId = 1, CenterName = "Centro1", CarId = 100, CarModel = "Auto1", UnitsSold = 5, PercentageOfGlobal = 50 },
                new() { CenterId = 2, CenterName = "Centro2", CarId = 101, CarModel = "Auto2", UnitsSold = 5, PercentageOfGlobal = 50 }
            };

            var mockData = data.AsQueryable().BuildMock();

            var paginatedList = await PaginatedList<PercentageGlobalCustom>.CreateAsync(mockData, 1, 10);

            _saleRepoMock
                .Setup(x => x.GetPercentageGlobalPaginatedAsync(
                    query.PageNumber,
                    query.PageSize,
                    query.SortOrder,
                    query.SortDirection,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedList);


            var result = await _handler.Handle(query, CancellationToken.None);

            var resultList = result.Items.ToList();
            Assert.AreEqual(2, result.TotalCount);
            Assert.AreEqual(1, resultList[0].CenterId);
            Assert.AreEqual("Centro1", resultList[0].CenterName);

            _saleRepoMock.Verify(x => x.GetPercentageGlobalPaginatedAsync(
                query.PageNumber,
                query.PageSize,
                query.SortOrder,
                query.SortDirection,
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Sales.Queries.GetSaleWithPagination;
using AutoMetricsService.Domain.Entities;
using Core.Framework.Aplication.Common.Wrappers;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Sales.Queries.GetSaleWithPagination
{
    [TestFixture]
    public class GetSaleWithPaginationQueryTests
    {
        private Mock<ISaleRepository> _saleRepoMock;
        private Mock<ILogger<GetSaleWithPaginationQuery>> _loggerMock;
        private GetSaleWithPaginationQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _saleRepoMock = new Mock<ISaleRepository>();
            _loggerMock = new Mock<ILogger<GetSaleWithPaginationQuery>>();

            _handler = new GetSaleWithPaginationQueryHandler(
                _saleRepoMock.Object,
                _loggerMock.Object
            );
        }

        [Test]
        public async Task Handle_ShouldReturnPaginatedSaleDto()
        {
            var query = new GetSaleWithPaginationQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Search = "",
                SortOrder = "Id",
                SortDirection = "asc"
            };

            var data = new List<Sale>
            {
                new Sale
                {
                    Id = 1,
                    CenterId = 10,
                    CarId = 20,
                    Units = 3,
                    UnitPrice = 100,
                    TotalTax = 30,
                    Total = 330
                },
                new Sale
                {
                    Id = 2,
                    CenterId = 11,
                    CarId = 21,
                    Units = 1,
                    UnitPrice = 200,
                    TotalTax = 40,
                    Total = 240
                }
            };

            var mockData = data.AsQueryable().BuildMock();

            var paginated = await PaginatedList<Sale>.CreateAsync(mockData, 1, 10);

            _saleRepoMock
                .Setup(x => x.GetAllPaginatedSearch(
                    query.PageNumber,
                    query.PageSize,
                    query.Search,
                    query.SortOrder,
                    query.SortDirection))
                .ReturnsAsync(paginated);

            var result = await _handler.Handle(query, CancellationToken.None);

            var resultList = result.Items.ToList();

            Assert.AreEqual(2, result.TotalCount);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual(3, resultList[0].Units);
            Assert.AreEqual(330, resultList[0].Total);

            _saleRepoMock.Verify(x => x.GetAllPaginatedSearch(
                query.PageNumber,
                query.PageSize,
                query.Search,
                query.SortOrder,
                query.SortDirection), Times.Once);
        }
    }
}

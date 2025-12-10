using Core.Framework.Aplication.Common.Wrappers;
using MockQueryable.Moq;

namespace Application.UnitTests.Common.Wrappers
{
    [TestFixture]
    public class PaginatedListTests
    {
        // Constructor básico
        [Test]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            var items = new List<int> { 1, 2, 3 };
            var paginatedList = new PaginatedList<int>(items, count: 10, pageNumber: 2, pageSize: 3);

            Assert.AreEqual(2, paginatedList.PageNumber);
            Assert.AreEqual(4, paginatedList.TotalPages);
            Assert.AreEqual(10, paginatedList.TotalCount);
            Assert.AreEqual(items, paginatedList.Items);
            Assert.IsTrue(paginatedList.HasPreviousPage);
            Assert.IsTrue(paginatedList.HasNextPage);
        }

        // HasPreviousPage y HasNextPage borde
        [Test]
        public void HasPreviousPageAndHasNextPage_ShouldReturnCorrectValues()
        {
            var items = new List<int> { 1, 2, 3 };

            var firstPage = new PaginatedList<int>(items, 10, 1, 3);
            Assert.IsFalse(firstPage.HasPreviousPage);
            Assert.IsTrue(firstPage.HasNextPage);

            var lastPage = new PaginatedList<int>(items, 10, 4, 3);
            Assert.IsTrue(lastPage.HasPreviousPage);
            Assert.IsFalse(lastPage.HasNextPage);
        }

        // CreateAsync should paginate correctly
        [Test]
        public async Task CreateAsync_ShouldReturnPaginatedListWithCorrectItems()
        {
            var sourceList = Enumerable.Range(1, 10)
                 .Select(i => new TestItem { Value = i })
                 .ToList();

            var mock = sourceList.AsQueryable().BuildMock();

            var paginated = await PaginatedList<TestItem>.CreateAsync(mock, pageNumber: 2, pageSize: 3);

            Assert.AreEqual(2, paginated.PageNumber);
            Assert.AreEqual(4, paginated.TotalPages);
            Assert.AreEqual(10, paginated.TotalCount);

            var expectedItems = new List<int> { 4, 5, 6 };

            // Extraer los Value de los objetos TestItem
            var actualValues = paginated.Items.Select(x => x.Value).ToList();

            CollectionAssert.AreEqual(expectedItems, actualValues);
        }

        // Paginar con página más allá del total
        [Test]
        public async Task CreateAsync_PageBeyondTotal_ShouldReturnEmptyItems()
        {
            var sourceList = Enumerable.Range(1, 5)
                .Select(i => new TestItem { Value = i })
                .ToList();

            var mock = sourceList.AsQueryable().BuildMock();

            var paginated = await PaginatedList<TestItem>.CreateAsync(mock, pageNumber: 3, pageSize: 5);

            Assert.AreEqual(3, paginated.PageNumber);
            Assert.AreEqual(1, paginated.TotalPages);
            Assert.AreEqual(5, paginated.TotalCount);
            Assert.AreEqual(0, paginated.Items.Count);
        }
    }

    public class TestItem
    {
        public int Value { get; set; }
    }
}

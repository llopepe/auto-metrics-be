using AutoMetricsService.Domain.Entities;
using AutoMetricsService.Infrastructure.Data;
using AutoMetricsService.Infrastructure.Repositories;
using Infrastructure.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.UnitTests.Repositories
{
    [TestFixture]
    public class SaleRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private SaleRepository _repository = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var mockAccessor = new Mock<IHttpContextAccessor>();
            _context = new ApplicationDbContext(options, mockAccessor.Object);

            var cars = TestDataSeeder.GetCars();
            var centers = TestDataSeeder.GetCenters();
            var sales = TestDataSeeder.GetSales(cars, centers);

            _context.Cars.AddRange(cars);
            _context.Centers.AddRange(centers);
            _context.Sales.AddRange(sales);
            _context.SaveChanges();

            _repository = new SaleRepository(_context);
        }

        [Test]
        public async Task GetAllPaginatedSearch_ShouldReturnAllSales_WhenNoFilter()
        {
            var result = await _repository.GetAllPaginatedSearch(1, 10, null, "Id", "asc");

            Assert.IsNotNull(result);
            Assert.AreEqual(15, result.TotalCount);
            Assert.AreEqual(1, result.PageNumber);
        }

        [Test]
        public async Task GetAllPaginatedSearch_ShouldReturnAllSales_WhenNoFilter_desc()
        {
            var result = await _repository.GetAllPaginatedSearch(1, 10, null, "Id", "desc");

            Assert.IsNotNull(result);
            Assert.AreEqual(15, result.TotalCount);
            Assert.AreEqual(1, result.PageNumber);
        }

        [Test]
        public async Task GetAllPaginatedSearch_ShouldFilterById()
        {
            var firstSale = _context.Sales.First();
            var result = await _repository.GetAllPaginatedSearch(0, 0, firstSale.Id.ToString(), null, null);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Any(s => s.Id == firstSale.Id));
        }

        [Test]
        public async Task GetTotalSalesVolumeAsync_ShouldReturnCorrectSum()
        {
            var total = await _repository.GetTotalSalesVolumeAsync(default);
            var expected = _context.Sales.Sum(s => s.Total);

            Assert.AreEqual(expected, total);
        }

        [Test]
        public async Task GetSalesByCenterPaginatedAsync_ShouldReturnGroupedSales()
        {
            var result = await _repository.GetSalesByCenterPaginatedAsync(null, 1, 10, "SalesVolume", "desc", default);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.All(r => r.SalesVolume > 0));
        }


        [Test]
        public async Task GetSalesByCenterPaginatedAsync_ShouldReturnGroupedSales_NullSortDirection()
        {
            var result = await _repository.GetSalesByCenterPaginatedAsync(null, 1, 10, null, null, default);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.All(r => r.SalesVolume > 0));
        }

        [Test]
        public async Task GetSalesByCenterPaginatedAsync_ShouldFilterByCenterId()
        {
            var center = _context.Centers.First();
            var result = await _repository.GetSalesByCenterPaginatedAsync(center.Id, 0, 0, "SalesVolume", "desc", default);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.All(r => r.CenterId == center.Id));
        }

        [Test]
        public async Task GetPercentageGlobalPaginatedAsync_ShouldReturnPercentages()
        {
            var result = await _repository.GetPercentageGlobalPaginatedAsync(1, 10, "CenterName", "asc", default);

            Assert.IsNotNull(result);
            var totalPercentage = result.Items.Sum(r => r.PercentageOfGlobal);
            Assert.IsTrue(totalPercentage <= 100);
        }

        [Test]
        public async Task GetPercentageGlobalPaginatedAsync_ShouldReturnPercentages_Desc()
        {
            var result = await _repository.GetPercentageGlobalPaginatedAsync(1, 10, "CenterName", "desc", default);

            Assert.IsNotNull(result);
            var totalPercentage = result.Items.Sum(r => r.PercentageOfGlobal);
            Assert.IsTrue(totalPercentage <= 100);
        }

        [Test]
        public async Task GetPercentageGlobalPaginatedAsync_ShouldReturnPercentages_NullSortDirection()
        {
            var result = await _repository.GetPercentageGlobalPaginatedAsync(0, 0, null, null, default);

            Assert.IsNotNull(result);
            var totalPercentage = result.Items.Sum(r => r.PercentageOfGlobal);
            Assert.IsTrue(totalPercentage <= 100);
        }

        [Test]
        public async Task AddUpdateDelete_ShouldWork()
        {
            var car = _context.Cars.First();
            var center = _context.Centers.First();

            // Add
            var newSale = new Sale
            {
                CarId = car.Id,
                CenterId = center.Id,
                Units = 2,
                UnitPrice = 100,
                Total = 200,
                TotalTax = 20,
                Date = DateTimeOffset.Now,
                UserName = "TestUser"
            };

            var added = await _repository.AddAsync(newSale);
            Assert.IsNotNull(added);
            Assert.AreEqual(200, added.Total);

            // Update
            added.Units = 3;
            added.Total = 300;
            var updated = await _repository.UpdateAsync(added);
            Assert.AreEqual(3, updated.Units);
            Assert.AreEqual(300, updated.Total);

            // Delete
            var deleted = await _repository.DeleteAsync(updated);
            var exists = await _repository.ExistsAsync(deleted.Id, default);
            Assert.IsFalse(exists);
        }
    }
}

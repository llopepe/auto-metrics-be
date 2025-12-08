using AutoMetricsService.Infrastructure.Data;
using AutoMetricsService.Infrastructure.Repositories;
using Infrastructure.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitTests.Repositories
{
    [TestFixture]
    public class CarRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private CarRepository _repository = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var mockAccessor = new Mock<IHttpContextAccessor>();

            _context = new ApplicationDbContext(options, mockAccessor.Object);

            _context.Cars.AddRange(TestDataSeeder.GetCars());
            _context.SaveChanges();

            _repository = new CarRepository(_context);
        }

        [Test]
        public async Task GetAllPaginatedSearch_ShouldReturnAllCars_WhenNoFilter()
        {
            var result = await _repository.GetAllPaginatedSearch(1, 10, null, "Id", "asc");

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.TotalCount);
            Assert.AreEqual(1, result.PageNumber);
            Assert.AreEqual(5, result.TotalCount);
        }

        [Test]
        public async Task GetAllPaginatedSearch_ShouldFilterByName()
        {
            var result = await _repository.GetAllPaginatedSearch(1, 10, "Car 1", "Id", "asc");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.TotalCount);
            Assert.AreEqual("Car 1", result.Items.First().Name);
        }

        [Test]
        public async Task GetAllPaginatedSearch_ShouldOrderDescending()
        {
            var result = await _repository.GetAllPaginatedSearch(1, 10, null, "Price", "desc");

            Assert.IsNotNull(result);
            var cars = result.Items.ToList();
            for (int i = 1; i < cars.Count; i++)
            {
                Assert.GreaterOrEqual(cars[i - 1].Price, cars[i].Price);
            }
        }

        [Test]
        public async Task ExistsAsync_ShouldReturnTrue_WhenCarExists()
        {
            bool exists = await _repository.ExistsAsync(1, default);
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task ExistsAsync_ShouldReturnFalse_WhenCarDoesNotExist()
        {
            bool exists = await _repository.ExistsAsync(999, default);
            Assert.IsFalse(exists);
        }
    }
}

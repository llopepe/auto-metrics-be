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
    public class CarTaxRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private CarTaxRepository _repository = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var mockAccessor = new Mock<IHttpContextAccessor>();
            _context = new ApplicationDbContext(options, mockAccessor.Object);

            var cars = TestDataSeeder.GetCars();
            var taxes = TestDataSeeder.GetCarTaxes(cars);

            _context.Cars.AddRange(cars);
            _context.CarTaxes.AddRange(taxes);
            _context.SaveChanges();

            _repository = new CarTaxRepository(_context);
        }

        [Test]
        public async Task GetAllPaginatedSearch_ShouldReturnAllCarTaxes_WhenNoFilter()
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
            var firstTax = _context.CarTaxes.First();
            var result = await _repository.GetAllPaginatedSearch(1, 10, firstTax.Name, "Id", "asc");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Any(t => t.Name == firstTax.Name));
        }

        [Test]
        public async Task GetAllPaginatedSearch_ShouldOrderDescending()
        {
            var result = await _repository.GetAllPaginatedSearch(1, 10, null, "Id", "desc");

            Assert.IsNotNull(result);
            var taxes = result.Items.ToList();
            for (int i = 1; i < taxes.Count; i++)
            {
                Assert.GreaterOrEqual(taxes[i - 1].Id, taxes[i].Id);
            }
        }

        [Test]
        public async Task GetTaxesByCarIdAsync_ShouldReturnCorrectTaxes()
        {
            var car = _context.Cars.First();
            var result = await _repository.GetTaxesByCarIdAsync(car.Id);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.All(t => t.CarId == car.Id));
        }

        [Test]
        public async Task AddUpdateDelete_ShouldWork()
        {
            // Add
            var car = _context.Cars.First();
            var newTax = new CarTax { CarId = car.Id, Name = "New Tax", Percentage = 5 };
            var added = await _repository.AddAsync(newTax);
            Assert.IsNotNull(added);
            Assert.AreEqual("New Tax", added.Name);

            // Update
            added.Percentage = 10;
            var updated = await _repository.UpdateAsync(added);
            Assert.AreEqual(10, updated.Percentage);

            // Delete
            var deleted = await _repository.DeleteAsync(updated);
            var exists = await _repository.ExistsAsync(deleted.Id, default);
            Assert.IsFalse(exists);
        }
    }
}

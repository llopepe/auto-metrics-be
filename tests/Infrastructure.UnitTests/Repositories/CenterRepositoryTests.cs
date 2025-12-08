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
    public class CenterRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private CenterRepository _repository = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var mockAccessor = new Mock<IHttpContextAccessor>();
            _context = new ApplicationDbContext(options, mockAccessor.Object);


            var centers = TestDataSeeder.GetCenters();
            _context.Centers.AddRange(centers);
            _context.SaveChanges();

            _repository = new CenterRepository(_context);
        }

        [Test]
        public async Task GetAllPaginatedSearch_ShouldReturnAllCenters_WhenNoFilter()
        {
            var result = await _repository.GetAllPaginatedSearch(1, 10, null, "Id", "asc");

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.TotalCount);
            Assert.AreEqual(1, result.PageNumber);
            Assert.AreEqual(3, result.TotalCount);
        }

        [Test]
        public async Task GetAllPaginatedSearch_ShouldFilterByName()
        {
            var firstCenter = _context.Centers.First();
            var result = await _repository.GetAllPaginatedSearch(1, 10, firstCenter.Name, "Id", "asc");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Any(c => c.Name == firstCenter.Name));
        }

        [Test]
        public async Task GetAllPaginatedSearch_ShouldOrderDescending()
        {
            var result = await _repository.GetAllPaginatedSearch(1, 10, null, "Id", "desc");

            Assert.IsNotNull(result);
            var centers = result.Items.ToList();
            for (int i = 1; i < centers.Count; i++)
            {
                Assert.GreaterOrEqual(centers[i - 1].Id, centers[i].Id);
            }
        }

        [Test]
        public async Task ExistsAsync_ShouldReturnTrue_WhenCenterExists()
        {
            var center = _context.Centers.First();
            bool exists = await _repository.ExistsAsync(center.Id, default);
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task ExistsAsync_ShouldReturnFalse_WhenCenterDoesNotExist()
        {
            bool exists = await _repository.ExistsAsync(999, default);
            Assert.IsFalse(exists);
        }

        [Test]
        public async Task AddUpdateDelete_ShouldWork()
        {
            // Add
            var newCenter = new Center { Name = "New Center" };
            var added = await _repository.AddAsync(newCenter);
            Assert.IsNotNull(added);
            Assert.AreEqual("New Center", added.Name);

            // Update
            added.Name = "Updated Center";
            var updated = await _repository.UpdateAsync(added);
            Assert.AreEqual("Updated Center", updated.Name);

            // Delete
            var deleted = await _repository.DeleteAsync(updated);
            var exists = await _repository.ExistsAsync(deleted.Id, default);
            Assert.IsFalse(exists);
        }
    }
}

using AutoMetricsService.Domain.Entities;
using AutoMetricsService.Infrastructure.Data;
using Core.Framework.Infrastructure.Repositories;
using Infrastructure.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.UnitTests.Repositories
{
    [TestFixture]
    public class BaseRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private BaseRepository<Car> _repository = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var mockAccessor = new Mock<IHttpContextAccessor>();

            _context = new ApplicationDbContext(options, mockAccessor.Object);
            _repository = new BaseRepository<Car>(_context);

            // Seed datos
            _context.Cars.AddRange(TestDataSeeder.GetCars());
            _context.SaveChanges();
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            var result = await _repository.GetAllAsync();
            Assert.AreEqual(5, result.Count);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenExists()
        {
            var car = await _repository.GetByIdAsync(1);
            Assert.IsNotNull(car);
            Assert.AreEqual(1, car.Id);
        }

        [Test]
        public async Task GetOneAsync_ShouldReturnEntity_WhenPredicateMatches()
        {
            var car = await _repository.GetOneAsync(c => c.Name.Contains("Car 1"));
            Assert.IsNotNull(car);
            Assert.AreEqual("Car 1", car.Name);
        }

        [Test]
        public async Task AddAsync_ShouldAddEntity()
        {
            var newCar = new Car { Name = "Car X", Price = 999 };
            var added = await _repository.AddAsync(newCar);

            var all = await _repository.GetAllAsync();
            Assert.Contains(added, all.ToList());
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            var car = await _repository.GetByIdAsync(1);
            car!.Price = 5555;
            await _repository.UpdateAsync(car);

            var updated = await _repository.GetByIdAsync(1);
            Assert.AreEqual(5555, updated!.Price);
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            var car = await _repository.GetByIdAsync(1);
            await _repository.DeleteAsync(car!);

            var deleted = await _repository.GetByIdAsync(1);
            Assert.IsNull(deleted);
        }

        [Test]
        public void ClearTracking_ShouldClearChangeTracker()
        {
            var car = _context.Cars.First();
            car.Price = 999;
            _repository.ClearTracking();

            var entry = _context.Entry(car);
            Assert.AreEqual(EntityState.Detached, entry.State);
        }

        [Test]
        public void Detach_ShouldDetachEntity()
        {
            var car = _context.Cars.First();
            _repository.Detach(car);

            var entry = _context.Entry(car);
            Assert.AreEqual(EntityState.Detached, entry.State);
        }

        [Test]
        public async Task GetAllByAsync_ShouldReturnFilteredEntities()
        {
            var result = await _repository.GetAllByAsync(c => c.Price > 0);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.All(c => c.Price > 0));
        }
    }

}

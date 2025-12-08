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
    public class UserRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private UserRepository _repository = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var mockAccessor = new Mock<IHttpContextAccessor>();
            _context = new ApplicationDbContext(options, mockAccessor.Object);

            // Seed datos: Users
            var users = TestDataSeeder.GetUsers();
            _context.Users.AddRange(users);
            _context.SaveChanges();

            _repository = new UserRepository(_context);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            var result = await _repository.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public async Task AddUpdateDelete_ShouldWork()
        {
            // Add
            var newUser = new User
            {
                Email = "newuser@test.com",
                PasswordHash = "hashedpassword",
                FullName = "New User",
                Roles = "User"
            };

            var added = await _repository.AddAsync(newUser);
            Assert.IsNotNull(added);
            Assert.AreEqual("New User", added.FullName);

            // Update
            added.FullName = "Updated User";
            var updated = await _repository.UpdateAsync(added);
            Assert.AreEqual("Updated User", updated.FullName);

            // Delete
            var deleted = await _repository.DeleteAsync(updated);
            var exists = await _repository.ExistsAsync(deleted.Id, default);
            Assert.IsFalse(exists);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            var user = _context.Users.First();
            var result = await _repository.GetByIdAsync(user.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result!.Email);
        }

        [Test]
        public async Task GetOneAsync_ShouldReturnUserByEmail()
        {
            var user = _context.Users.First();
            var result = await _repository.GetOneAsync(u => u.Email == user.Email);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.FullName, result!.FullName);
        }
    }
}

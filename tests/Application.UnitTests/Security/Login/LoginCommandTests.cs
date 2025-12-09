using AutoMetricsService.Application.Interfaces.Auth;
using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Security.Login;
using AutoMetricsService.Domain.Entities;
using Core.Framework.Aplication.Common.Exceptions;
using Core.Framework.Aplication.Common.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Application.UnitTests.Security.Login
{
    public class LoginCommandTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IJwtTokenService> _jwtServiceMock;
        private Mock<ILogger<LoginCommandRequest>> _loggerMock;

        private JwtSettings _jwtSettings;
        private LoginCommandRequest _handler;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _jwtServiceMock = new Mock<IJwtTokenService>();
            _loggerMock = new Mock<ILogger<LoginCommandRequest>>();

            _jwtSettings = new JwtSettings
            {
                Key = "SuperSecretKeyForUnitTesting123456!",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpirationInMinutes = 30
            };

            _handler = new LoginCommandRequest(
                _loggerMock.Object,
                _userRepoMock.Object,
                _jwtSettings,
                _jwtServiceMock.Object
            );
        }

        // Login exitoso
        [Test]
        public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                Roles = "Admin,User"
            };

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, "123456");

            _userRepoMock
               .Setup(r => r.GetOneAsync(It.IsAny<Expression<Func<User, bool>>>()))
               .ReturnsAsync(user);


            _jwtServiceMock
                .Setup(j => j.GenerateToken(
                    "1",
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<IDictionary<string, string>>()
                ))
                .Returns("FAKE_TOKEN");

            var command = new LoginCommand
            {
                Email = "test@example.com",
                Password = "123456"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual("FAKE_TOKEN", result.Data.Token);
            Assert.AreEqual(user.Roles, result.Data.Roles);
            Assert.Greater(result.Data.ExpiresAt, DateTime.UtcNow);
        }

        // Usuario no encontrado
        [Test]
        public void Handle_ShouldThrowAuthenticationException_WhenUserNotFound()
        {
            _userRepoMock
                .Setup(r => r.GetOneAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User?)null);

            var command = new LoginCommand
            {
                Email = "notfound@example.com",
                Password = "123"
            };

            Assert.ThrowsAsync<AuthenticationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        // Contraseña inválida
        [Test]
        public void Handle_ShouldThrowAuthenticationException_WhenPasswordIsInvalid()
        {
            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                Roles = "Admin"
            };

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, "CORRECT_PASSWORD");

            _userRepoMock
              .Setup(r => r.GetOneAsync(It.IsAny<Expression<Func<User, bool>>>()))
              .ReturnsAsync(user);

            var command = new LoginCommand
            {
                Email = "test@example.com",
                Password = "WRONG_PASSWORD"
            };

            var handler = new LoginCommandRequest(
                      _loggerMock.Object,
                      _userRepoMock.Object,
                      _jwtSettings,
                      _jwtServiceMock.Object
                );

            Assert.ThrowsAsync<AuthenticationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        // Token generation correctamente
        [Test]
        public async Task Handle_ShouldCallJwtService_WithRoles()
        {
            // Arrange
            var user = new User
            {
                Id = 5,
                Email = "roles@test.com",
                Roles = "Manager, Auditor"
            };

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, "pass");

            _userRepoMock
                .Setup(r => r.GetOneAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);


            _jwtServiceMock
            .Setup(j => j.GenerateToken(
                "5",
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<IDictionary<string, string>>()
            ))
            .Returns("TOKEN123");

            var command = new LoginCommand
            {
                Email = user.Email,
                Password = "pass"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.AreEqual("TOKEN123", result.Data.Token);

            _jwtServiceMock.Verify(j =>
                j.GenerateToken("5",
                    It.Is<IEnumerable<string>>(r => r.Contains("Manager") && r.Contains("Auditor")),
                    It.IsAny<IDictionary<string, string>>()
                ),
                Times.Once);

        }
    }
}

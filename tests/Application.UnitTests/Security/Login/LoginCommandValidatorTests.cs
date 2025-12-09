using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Security.Login;
using AutoMetricsService.Domain.Entities;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Security.Login
{
    public class LoginCommandValidatorTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private LoginCommandValidator _validator;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _validator = new LoginCommandValidator(_userRepoMock.Object);
        }

        // Email vacío
        [Test]
        public async Task ShouldHaveError_WhenEmailIsEmpty()
        {
            var model = new LoginCommand { Email = "", Password = "123" };

            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("El email es obligatorio.");
        }

        // Email formato inválido
        [Test]
        public async Task ShouldHaveError_WhenEmailIsInvalid()
        {
            var model = new LoginCommand { Email = "not-an-email", Password = "123" };

            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("El email no tiene un formato válido.");
        }

        // Email válido pero usuario NO existe
        [Test]
        public async Task ShouldHaveError_WhenUserDoesNotExist()
        {
            _userRepoMock
                .Setup(r => r.GetOneAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            var model = new LoginCommand
            {
                Email = "test@example.com",
                Password = "123"
            };

            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("El usuario no existe en nuestros registros");
        }

        // Email válido y usuario existe
        [Test]
        public async Task ShouldNotHaveError_WhenUserExists()
        {
            var user = new User
            {
                Id = 1,
                Email = "test@example.com"
            };

            _userRepoMock
                .Setup(r => r.GetOneAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            var model = new LoginCommand
            {
                Email = "test@example.com",
                Password = "123"
            };

            var result = await _validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        // Password vacío
        [Test]
        public async Task ShouldHaveError_WhenPasswordIsEmpty()
        {
            var model = new LoginCommand
            {
                Email = "test@example.com",
                Password = ""
            };

            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorMessage("La contraseña es obligatoria.");
        }

        // Validación exitosa
        [Test]
        public async Task ShouldPassValidation_WhenInputIsValid()
        {
            var user = new User
            {
                Id = 1,
                Email = "test@example.com"
            };

            _userRepoMock
                .Setup(r => r.GetOneAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            var model = new LoginCommand
            {
                Email = "test@example.com",
                Password = "123"
            };

            var result = await _validator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

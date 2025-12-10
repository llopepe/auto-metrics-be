using AutoMetricsService.Api.Controllers;
using AutoMetricsService.Application.Security.Dto;
using AutoMetricsService.Application.Security.Login;
using Core.Framework.Aplication.Common.Enums;
using Core.Framework.Aplication.Common.Wrappers;
using MediatR;
using Moq;

namespace Application.Functional.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private Mock<ISender> _mockSender;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _mockSender = new Mock<ISender>();
            _controller = new AuthController();
        }

        [Test]
        public async Task Login_ShouldReturnResultResponseWithToken_WhenCommandIsValid()
        {
            var command = new LoginCommand
            {
                Email = "testuser",
                Password = "Password123!"
            };

            var expectedResponse = ResultResponse<LoginResponseDto>.Ok(new LoginResponseDto
            {
                Token = "mocked-jwt-token",
                ExpiresAt = System.DateTime.UtcNow.AddHours(1)
            });

            _mockSender
                .Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.Login(_mockSender.Object, command);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ResultResponse<LoginResponseDto>>(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("mocked-jwt-token", result.Data.Token);

            _mockSender.Verify(s => s.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Login_ShouldReturnError_WhenCommandFails()
        {

            var command = new LoginCommand
            {
                Email = "wronguser",
                Password = "wrongpassword"
            };

            var errorResponse = new ResultResponse<LoginResponseDto>();
            errorResponse.AddError(new Error(ErrorCodeResponse.Unauthorized, "Invalid credentials", ""));

            _mockSender
                .Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResponse);

            var result = await _controller.Login(_mockSender.Object, command);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ResultResponse<LoginResponseDto>>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid credentials", result.Errors[0].Description);

            _mockSender.Verify(s => s.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

using AutoMetricsService.Application.Security.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Security.Dto
{
    public class LoginResponseDtoTests
    {
        [Test]
        public void Constructor_ShouldInitializeProperties_WithDefaultValues()
        {
            var dto = new LoginResponseDto();

            Assert.IsNotNull(dto);
            Assert.AreEqual(string.Empty, dto.Token);
            Assert.AreEqual(string.Empty, dto.Roles);
            Assert.AreEqual(default(DateTime), dto.ExpiresAt);
        }

        [Test]
        public void Properties_ShouldSetAndGetValuesCorrectly()
        {
            var expectedToken = "abc123";
            var expectedRoles = "Admin,User";
            var expectedDate = DateTime.UtcNow.AddHours(1);

            var dto = new LoginResponseDto();

            dto.Token = expectedToken;
            dto.Roles = expectedRoles;
            dto.ExpiresAt = expectedDate;

            Assert.AreEqual(expectedToken, dto.Token);
            Assert.AreEqual(expectedRoles, dto.Roles);
            Assert.AreEqual(expectedDate, dto.ExpiresAt);
        }

        [Test]
        public void TwoDtos_WithSameValues_ShouldBeEquivalent()
        {
            var dt = DateTime.UtcNow;

            var dto1 = new LoginResponseDto
            {
                Token = "token-example",
                ExpiresAt = dt,
                Roles = "Manager"
            };

            var dto2 = new LoginResponseDto
            {
                Token = "token-example",
                ExpiresAt = dt,
                Roles = "Manager"
            };

            Assert.AreEqual(dto1.Token, dto2.Token);
            Assert.AreEqual(dto1.Roles, dto2.Roles);
            Assert.AreEqual(dto1.ExpiresAt, dto2.ExpiresAt);
        }

        [Test]
        public void ExpiresAt_ShouldBeValidFutureDate()
        {
            var dto = new LoginResponseDto
            {
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            Assert.Greater(dto.ExpiresAt, DateTime.UtcNow);
        }
    }
}

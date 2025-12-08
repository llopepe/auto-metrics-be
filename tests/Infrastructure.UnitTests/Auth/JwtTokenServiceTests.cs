using AutoMetricsService.Infrastructure.Auth;
using Core.Framework.Aplication.Common.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitTests.Auth
{
    [TestFixture]
    public class JwtTokenServiceTests
    {
        private JwtTokenService _service = null!;
        private JwtSettings _jwtSettings = null!;

        [SetUp]
        public void Setup()
        {
            _jwtSettings = new JwtSettings
            {
                Key = "ThisIsASecretKeyForTestingPurposes123!",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpirationInMinutes = 60
            };

            _service = new JwtTokenService(_jwtSettings);
        }

        [Test]
        public void GenerateToken_ShouldReturnNonEmptyToken()
        {
            var token = _service.GenerateToken("user123", new[] { "Admin" });

            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);
        }

        [Test]
        public void GenerateToken_ShouldIncludeSubAndRoles()
        {
            var tokenString = _service.GenerateToken("user123", new[] { "Admin", "User" });
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            var subClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            Assert.IsNotNull(subClaim);
            Assert.AreEqual("user123", subClaim!.Value);

            var roleClaims = token.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            CollectionAssert.AreEquivalent(new[] { "Admin", "User" }, roleClaims);
        }

        [Test]
        public void GenerateToken_ShouldIncludeAdditionalClaims()
        {
            var additional = new Dictionary<string, string> { { "custom", "value123" } };
            var tokenString = _service.GenerateToken("user123", new[] { "Admin" }, additional);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            var customClaim = token.Claims.FirstOrDefault(c => c.Type == "custom");
            Assert.IsNotNull(customClaim);
            Assert.AreEqual("value123", customClaim!.Value);
        }

        [Test]
        public void GenerateToken_ShouldSetIssuerAndAudience()
        {
            var tokenString = _service.GenerateToken("user123", new[] { "Admin" });
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            Assert.AreEqual(_jwtSettings.Issuer, token.Issuer);
            Assert.AreEqual(_jwtSettings.Audience, token.Audiences.First());
        }

        [Test]
        public void GenerateToken_ShouldSetExpiration()
        {
            var tokenString = _service.GenerateToken("user123", new[] { "Admin" });
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            var expected = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);
            var diff = (token.ValidTo - expected).TotalSeconds;
            Assert.LessOrEqual(Math.Abs(diff), 5);
        }
    }
}

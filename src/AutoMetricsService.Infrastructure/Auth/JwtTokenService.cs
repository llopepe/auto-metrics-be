using AutoMetricsService.Application.Interfaces.Auth;
using Core.Framework.Aplication.Common.Security;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutoMetricsService.Infrastructure.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwt;

        public JwtTokenService(JwtSettings jwtSettings)
        {
            _jwt = jwtSettings;
        }

        public string GenerateToken(string userId, IEnumerable<string> roles, IDictionary<string, string>? additionalClaims = null)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            // roles
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // extra claims
            if (additionalClaims != null)
            {
                foreach (var kv in additionalClaims)
                {
                    claims.Add(new Claim(kv.Key, kv.Value));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpirationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

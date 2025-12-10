using System.Collections.Generic;

namespace AutoMetricsService.Application.Interfaces.Auth
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, IEnumerable<string> roles, IDictionary<string, string> additionalClaims = null);
    }
}

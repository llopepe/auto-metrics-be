using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Interfaces.Auth
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, IEnumerable<string> roles, IDictionary<string, string> additionalClaims = null);
    }
}

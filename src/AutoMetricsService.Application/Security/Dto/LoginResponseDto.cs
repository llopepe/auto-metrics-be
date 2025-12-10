using System;

namespace AutoMetricsService.Application.Security.Dto
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string Roles { get; set; } = string.Empty;
    }
}

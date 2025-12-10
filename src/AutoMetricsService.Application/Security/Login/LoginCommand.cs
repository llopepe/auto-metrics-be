using AutoMetricsService.Application.Interfaces.Auth;
using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Application.Security.Dto;
using AutoMetricsService.Domain.Entities;
using Core.Framework.Aplication.Common.Exceptions;
using Core.Framework.Aplication.Common.Security;
using Core.Framework.Aplication.Common.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Security.Login
{
    public record LoginCommand : IRequest<ResultResponse<LoginResponseDto>>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }


    public class LoginCommandRequest : IRequestHandler<LoginCommand, ResultResponse<LoginResponseDto>>
    {
        private readonly ILogger<LoginCommandRequest> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtService;
        private readonly JwtSettings _jwtSettings;
        private readonly PasswordHasher<User> _hasher;

        public LoginCommandRequest(ILogger<LoginCommandRequest> logger,
                                   IUserRepository userRepository, JwtSettings jwtSettings, IJwtTokenService jwtService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
            _hasher = new PasswordHasher<User>();
            _jwtService = jwtService;

        }

        public async Task<ResultResponse<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            //Buscamos el userId para generar el token
            var user = await _userRepository.GetOneAsync(u => u.Email == request.Email);

            if (user == null)
                throw new AuthenticationException("Credenciales inválidas");

            // Verificamos la contraseña
            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (verify != PasswordVerificationResult.Success)
                throw new AuthenticationException("Credenciales inválidas");

            // Roles
            var roles = string.IsNullOrWhiteSpace(user.Roles) ? new List<string>() : user.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
            // Generamos el token
            var token = _jwtService.GenerateToken(user.Id.ToString(), roles);

            return new LoginResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                Roles = user.Roles
            };

        }

    }


}

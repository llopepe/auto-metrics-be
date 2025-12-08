using AutoMetricsService.Application.Security.Dto;
using AutoMetricsService.Application.Security.Login;
using Core.Framework.Aplication.Common.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AutoMetricsService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login de usuario", Description = "Obtiene el token JWT, para utilizar lo otros controladores.")]
        public async Task<ResultResponse<LoginResponseDto>> Login(ISender sender, [FromBody] LoginCommand command)
        {
            // llama al comando
            return await sender.Send(command);
        }
    }
}

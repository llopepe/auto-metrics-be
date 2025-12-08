using AutoMetricsService.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Security.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        private readonly IUserRepository _userRepository;
        public LoginCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El email no tiene un formato válido.")
                .MustAsync(UserExits).WithMessage("El usuario no existe en nuestros registros");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.");

        }

        private async Task<bool> UserExits(string email, CancellationToken token)
        {
            var user = await _userRepository.GetOneAsync(u => u.Email == email);

            return user != null;
        }


    }
}

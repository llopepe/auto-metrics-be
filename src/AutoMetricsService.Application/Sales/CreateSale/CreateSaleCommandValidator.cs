using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Sales.CreateSale
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {

        private readonly ICarRepository _carRepository;
        private readonly ICenterRepository _centerRepository;

        public CreateSaleCommandValidator(ICarRepository carRepository, ICenterRepository centerRepository)
        {

            _carRepository = carRepository;
            _centerRepository = centerRepository;

            RuleFor(x => x.CenterId)
                     .GreaterThan(0)
                     .WithMessage("CenterId es obligatorio.")
                     .MustAsync(CenterExists)
                     .WithMessage("El CenterId especificado no existe en la base de datos.");

            RuleFor(x => x.CarId)
                .GreaterThan(0)
                .WithMessage("CarId es obligatorio.")
                .MustAsync(CarExists)
                .WithMessage("El CarId especificado no existe en la base de datos.");

            RuleFor(x => x.Units)
                .GreaterThan(0)
                .WithMessage("Units debe ser mayor a 0.");
            _centerRepository = centerRepository;

        }


        private async Task<bool> CenterExists(int centerId, CancellationToken ct)
        {
            return await _centerRepository.ExistsAsync(centerId, ct);
        }

        private async Task<bool> CarExists(int carId, CancellationToken ct)
        {
            return await _carRepository.ExistsAsync(carId,ct);
        }
    }
}

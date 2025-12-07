using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Domain.Entities;
using AutoMetricsService.Domain.EntitiesCustom;
using AutoMetricsService.Domain.Events;
using Core.Framework.Aplication.Common.Exceptions;
using Core.Framework.Aplication.Common.Wrappers;
using Core.Framework.Aplication.Interfaces.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<ResultResponse<int>>
    {
        public int CenterId { get; set; }
        public int CarId { get; set; }
        public int Units { get; set; }
    }

    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, ResultResponse<int>>
    {

        private readonly ILogger<CreateSaleCommandHandler> _logger;
        private readonly ISaleRepository _saleRepository;
        private readonly ICarRepository _carRepository;
        private readonly ICarTaxRepository  _carTaxRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSaleCommandHandler(ISaleRepository saleRepository, IUnitOfWork unit, ILogger<CreateSaleCommandHandler> logger,
                                          ICarRepository carRepository, ICarTaxRepository carTaxRepository)
        { 
            _saleRepository = saleRepository;
            _unitOfWork = unit; 
            _logger = logger;
            _carRepository = carRepository;
            _carTaxRepository = carTaxRepository;
        }


        public async Task<ResultResponse<int>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
           
            _logger.LogInformation("Ejecutando {Command}", request.GetType().Name);

            // Calcular total (precio base * unidades + impuesto si los tiene)
            var saleAmount = await CalculateSaleAmountsAsync(request.CarId, request.Units);

            // Creo la entidad con los datos recibidos y agregar datos faltantes
            var entity = new Sale
            {
                CenterId = request.CenterId,
                CarId = request.CarId,
                Units = request.Units,
                Date = DateTime.Now,
                UnitPrice = saleAmount.UnitPrice, //Precio unitario sin impuestos
                TotalTax = saleAmount.TotalTax, //Impuesto total aplicado
                Total = saleAmount.Total, //Total con impuestos
                UserName = "system" // En un caso real, obtener el usuario desde el contexto de seguridad
            };

            try
            {
                // Guardar la entidad
                await _saleRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Venta Creada Exitosamente - {SaleId}", entity.Id);

                entity!.AddDomainEvent(new SaleCreatedEvent(entity));
                _logger.LogInformation("Evento SaleCreatedEvent agregado para la Venta - {SaleId}", entity.Id);
                return entity.Id;

            }
            catch (Exception ex)
            {
                // En caso de error, loguear y lanzar excepción personalizada
                _logger.LogError(ex, "Error al Guardar los datos");
                throw new CommandException("Error al Guardar los datos", ex);
            }
        }


        private async Task<SaleAmountResultCustom> CalculateSaleAmountsAsync(int carId, int units)
        {
            if (units <= 0)
                throw new ArgumentException("Units must be greater than zero.", nameof(units));

            // Obtener el auto
            var car = await _carRepository.GetByIdAsync(carId)
                      ?? throw new Exception($"Car with ID {carId} not found.");

            //Precio por unidad sin impuestos
            decimal baseUnitPrice = car.Price;

            // Calcular total base sin impuestos
            decimal baseTotal = baseUnitPrice * units;

            // Obtener impuestos asociados
            var taxes = await _carTaxRepository.GetTaxesByCarIdAsync(carId);

            decimal totalWithTaxes = baseTotal;

            // Si hay impuestos, los aplico uno por uno sobre el total base
            if (taxes is { Count: > 0 })
            {
                foreach (var tax in taxes)
                {
                    // Evita nulos y porcentajes no válidos
                    if (tax is not null && tax.Percentage > 0)
                    {
                        totalWithTaxes += baseTotal * (tax.Percentage / 100m);
                    }
                }
            }

            // Redondeo a 2 decimales
            totalWithTaxes = Math.Round(totalWithTaxes, 2);

            // Cálculos finales
            decimal totalTax = totalWithTaxes - baseTotal;
  
            return new SaleAmountResultCustom
            {
                UnitPrice = Math.Round(car.Price, 2),
                TotalTax = Math.Round(totalTax, 2),
                Total = totalWithTaxes
            };
        }


    }
}

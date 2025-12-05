using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Domain.Entities;
using AutoMetricsService.Domain.Events;
using Core.Framework.Aplication.Common.Exceptions;
using Core.Framework.Aplication.Common.Wrappers;
using Core.Framework.Aplication.Interfaces.Data;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly IUnitOfWork _unitOfWork;

        public CreateSaleCommandHandler(ISaleRepository saleRepository, IUnitOfWork unit, ILogger<CreateSaleCommandHandler> logger)
        { 
            _saleRepository = saleRepository;
            _unitOfWork = unit; 
            _logger = logger;
        }


        public async Task<ResultResponse<int>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
           
            _logger.LogInformation("Ejecutando {Command}", request.GetType().Name);

            // Creo la entidad con los datos recibidos y agregar datos faltantes
            var entity = new Sale
            {
                CenterId = request.CenterId,
                CarId = request.CarId,
                Units = request.Units,
                Date = DateTime.Now
            };

            try
            {
                await _saleRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Venta Creada Exitosamente - {SaleId}", entity.Id);

                entity!.AddDomainEvent(new SaleCreatedEvent(entity));
                _logger.LogInformation("Evento SaleCreatedEvent agregado para la Venta - {SaleId}", entity.Id);
                return entity.Id;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Guardar los datos");
                throw new CommandException("Error al Guardar los datos", ex);
            }
        }
    }
}

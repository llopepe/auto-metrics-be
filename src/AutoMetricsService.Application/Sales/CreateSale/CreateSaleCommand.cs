using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Domain.Entities;
using AutoMetricsService.Domain.Events;
using Core.Framework.Aplication.Common.Exceptions;
using Core.Framework.Aplication.Common.Wrappers;
using Core.Framework.Aplication.Interfaces.Data;
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


        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSaleCommandHandler(ISaleRepository saleRepository, IUnitOfWork unit)
        { 
            _saleRepository = saleRepository;
            _unitOfWork = unit; 
        }


        public async Task<ResultResponse<int>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {

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

                entity!.AddDomainEvent(new SaleCreatedEvent(entity));
                return entity.Id;

            }
            catch (Exception ex)
            {
                throw new CommandException("Error al Guardar los datos", ex);
            }
        }
    }
}

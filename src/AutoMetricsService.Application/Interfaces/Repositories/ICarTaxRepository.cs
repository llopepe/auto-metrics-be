using AutoMetricsService.Domain.Entities;
using Core.Framework.Aplication.Common.Wrappers;
using Core.Framework.Aplication.Interfaces.Repositories.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Interfaces.Repositories
{
    public interface ICarTaxRepository : IBaseRepository<CarTax>
    {
        Task<bool> ExistsAsync(int id, CancellationToken ct);
        Task<PaginatedList<CarTax>> GetAllPaginatedSearch(int page, int size, string search, string sortOrder, string sortDirection);
        Task<IList<CarTax>> GetTaxesByCarIdAsync(int carId);
    }
}

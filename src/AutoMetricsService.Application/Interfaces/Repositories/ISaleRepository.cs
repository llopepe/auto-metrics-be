using AutoMetricsService.Domain.Entities;
using AutoMetricsService.Domain.EntitiesCustom;
using Core.Framework.Aplication.Common.Wrappers;
using Core.Framework.Aplication.Interfaces.Repositories.Base;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Interfaces.Repositories
{
    public interface ISaleRepository : IBaseRepository<Sale>
    {
        Task<bool> ExistsAsync(int id, CancellationToken ct);
        Task<PaginatedList<Sale>> GetAllPaginatedSearch(int page, int size, string search, string sortOrder, string sortDirection);
        Task<PaginatedList<PercentageGlobalCustom>> GetPercentageGlobalPaginatedAsync(int page, int size, string sortOrder, string sortDirection, CancellationToken cancellationToken);
        Task<PaginatedList<SalesVolumeCenterCustom>> GetSalesByCenterPaginatedAsync(int? centerId, int page, int size, string sortOrder, string sortDirection, CancellationToken cancellationToken);
        Task<decimal> GetTotalSalesVolumeAsync(CancellationToken cancellationToken);
    }
}

using AutoMetricsService.Domain.Entities;
using Core.Framework.Aplication.Common.Wrappers;
using Core.Framework.Aplication.Interfaces.Repositories.Base;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Interfaces.Repositories
{
    public interface ICarRepository : IBaseRepository<Car>
    {
        Task<bool> ExistsAsync(int id, CancellationToken ct);
        Task<PaginatedList<Car>> GetAllPaginatedSearch(int page, int size, string search, string sortOrder, string sortDirection);
    }
}

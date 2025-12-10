using AutoMetricsService.Domain.Entities;
using Core.Framework.Aplication.Interfaces.Repositories.Base;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> ExistsAsync(int id, CancellationToken ct);
    }
}

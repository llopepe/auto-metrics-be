using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Domain.Entities;
using AutoMetricsService.Infrastructure.Data;
using Core.Framework.Aplication.Common.Wrappers;
using Core.Framework.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private ApplicationDbContext AppContext => (ApplicationDbContext)_context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct)
        {
            return await _dbSet.AnyAsync(c => c.Id == id, ct);
        }

    }
}

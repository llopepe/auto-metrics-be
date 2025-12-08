using AutoMetricsService.Domain.Entities;
using Core.Framework.Aplication.Common.Wrappers;
using Core.Framework.Aplication.Interfaces.Repositories.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> ExistsAsync(int id, CancellationToken ct);
    }
}

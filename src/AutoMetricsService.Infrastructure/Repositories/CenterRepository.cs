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
    public class CenterRepository : BaseRepository<Center>, ICenterRepository
    {
        private ApplicationDbContext AppContext => (ApplicationDbContext)_context;

        public CenterRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Center>> GetAllPaginatedSearch(int page, int size, string search, string sortOrder, string sortDirection)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? 10 : size;

            IQueryable<Center> query = _dbSet.AsNoTracking();

            // --- FILTRO ---
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToUpper();

                query = query.Where(c =>
                    EF.Functions.Like(c.Name.ToUpper(), $"%{search}%") ||
                    c.Id.ToString().Contains(search)
                );
            }

            // --- ORDEN DINÁMICO ---
            // Lista de propiedades permitidas para ordenar
            var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Id",
                "Name"
            };

            string sortColumn = allowedSortColumns.Contains(sortOrder ?? "")
                ? sortOrder!
                : "Id"; // default

            bool isDesc = sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false;

            query = isDesc
                ? query.OrderByDescending(e => EF.Property<object>(e, sortColumn))
                : query.OrderBy(e => EF.Property<object>(e, sortColumn));

            // --- PAGINACIÓN ---
            return await PaginatedList<Center>.CreateAsync(query, page, size);
        }

        public async Task<bool> ExistsAsync(int centerId, CancellationToken ct)
        {
            return await _dbSet.AnyAsync(c => c.Id == centerId);
        }
    }
}

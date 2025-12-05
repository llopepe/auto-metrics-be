using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Domain.Entities;
using AutoMetricsService.Domain.EntitiesCustom;
using AutoMetricsService.Infrastructure.Data;
using Core.Framework.Aplication.Common.Wrappers;
using Core.Framework.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMetricsService.Infrastructure.Repositories
{
    public class SaleRepository : BaseRepository<Sale>, ISaleRepository
    {
        private ApplicationDbContext AppContext => (ApplicationDbContext)_context;

        public SaleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Sale>> GetAllPaginatedSearch(int page, int size, string search, 
                                                                    string sortOrder, string sortDirection)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? 10 : size;

            IQueryable<Sale> query = AppContext.Sales.AsNoTracking();

            //Filtro de búsqueda
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToUpper();

                query = query.Where(c =>
                    c.Id.ToString().Contains(search)
                );
            }

            //Orden dinámico
            // Lista de propiedades permitidas para ordenar
            var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Id"
            };

            string sortColumn = allowedSortColumns.Contains(sortOrder ?? "")
                ? sortOrder!
                : "Id"; // default

            bool isDesc = sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false;

            query = isDesc
                ? query.OrderByDescending(e => EF.Property<object>(e, sortColumn))
                : query.OrderBy(e => EF.Property<object>(e, sortColumn));

            //Paginado
            return await PaginatedList<Sale>.CreateAsync(query, page, size);
        }
    
        public async Task<decimal> GetTotalSalesVolumeAsync(CancellationToken cancellationToken)
        {
            return await AppContext.Sales.AsNoTracking().SumAsync(s => s.Total, cancellationToken);
        }

        public async Task<PaginatedList<SalesVolumeCenterCustom>> GetSalesByCenterPaginatedAsync( int? centerId, int page,
                                                                                    int size,
                                                                                    string sortOrder,
                                                                                    string sortDirection,
                                                                                    CancellationToken cancellationToken)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? 10 : size;

            var query =
                from s in AppContext.Sales.AsNoTracking()
                join c in AppContext.Centers.AsNoTracking()
                    on s.CenterId equals c.Id
                select new SalesVolumeCenterCustom
                {
                    CenterId = s.CenterId,
                    CenterName = c.Name,
                    SalesVolume = s.Total
                };

            //Filtro opcional por centro
            if (centerId.HasValue)
            {
                query = query.Where(x => x.CenterId == centerId.Value);
            }

            //Agrupar por centro
            var groupedQuery = query
                .GroupBy(x => new { x.CenterId, x.CenterName })
                .Select(g => new SalesVolumeCenterCustom
                {
                    CenterId = g.Key.CenterId,
                    CenterName = g.Key.CenterName,
                    SalesVolume = g.Sum(x => x.SalesVolume)
                });

            //Orden dinámico
            var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "CenterId",
                "CenterName",
                "SalesVolume"
            };

            string sortColumn = allowedSortColumns.Contains(sortOrder ?? "")
                ? sortOrder!
                : "CenterId"; // default

            bool isDesc = sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false;

            groupedQuery = isDesc
                ? groupedQuery.OrderByDescending(e => EF.Property<object>(e, sortColumn))
                : groupedQuery.OrderBy(e => EF.Property<object>(e, sortColumn));

            // Paginado
            return await PaginatedList<SalesVolumeCenterCustom>.CreateAsync(groupedQuery,page,size );
        }

        public async Task<PaginatedList<PercentageGlobalCustom>> GetPercentageGlobalPaginatedAsync( int page,
                                                                                  int size,
                                                                                  string sortOrder,
                                                                                  string sortDirection,
                                                                                  CancellationToken cancellationToken)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? 10 : size;

            //Suma los totales globales de unidades vendidas
            var totalUnits = await AppContext.Sales
                    .AsNoTracking()
                    .Select(s => s.Units)
                    .SumAsync(cancellationToken);


            var percentageGlobalQuery =
                                     from s in AppContext.Sales.AsNoTracking()
                                     join c in AppContext.Centers.AsNoTracking() on s.CenterId equals c.Id
                                     join car in AppContext.Cars.AsNoTracking() on s.CarId equals car.Id
                                     group new { s, c, car } by new { s.CenterId, s.CarId, CenterName = c.Name, CarModel = car.Name } into g
                                     select new PercentageGlobalCustom
                                     {
                                         CenterId = g.Key.CenterId,
                                         CenterName = g.Key.CenterName,
                                         CarId = g.Key.CarId,
                                         CarModel = g.Key.CarModel,
                                         UnitsSold = g.Sum(x => x.s.Units),
                                         PercentageOfGlobal = Math.Round((decimal)g.Sum(x => x.s.Units) * 100 / totalUnits,4)
                                     };

            percentageGlobalQuery = percentageGlobalQuery
                                        .OrderBy(x => x.CenterName)
                                        .ThenByDescending(x => x.PercentageOfGlobal)
                                        .AsQueryable();

            //Orden dinámico
            var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "CenterId",
                "CenterName",
                "CarId",
                "CarModel",
                "UnitsSold",
                "PercentageOfGlobal"
            };

            string sortColumn = allowedSortColumns.Contains(sortOrder ?? "")
                ? sortOrder!
                : "CenterId"; // default

            bool isDesc = sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false;

            percentageGlobalQuery = isDesc
                ? percentageGlobalQuery.OrderByDescending(e => EF.Property<object>(e, sortColumn))
                : percentageGlobalQuery.OrderBy(e => EF.Property<object>(e, sortColumn));

            // Paginado
            return await PaginatedList<PercentageGlobalCustom>.CreateAsync(percentageGlobalQuery, page, size);
        }
    }
}

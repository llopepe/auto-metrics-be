using Core.Framework.Aplication.Common.Wrappers;
using Mapster;
using System.Collections.Generic;

namespace AutoMetricsService.Application.Common.Extensions
{
    public static class PaginatedListExtensions
    {
        public static PaginatedList<TDest> AdaptPaginated<TSource, TDest>(
        this PaginatedList<TSource> source)
        {
            var mappedItems = source.Items
                .Adapt<List<TDest>>()
                .AsReadOnly();

            return new PaginatedList<TDest>(
                mappedItems,
                source.TotalCount,
                source.PageNumber,
                source.TotalPages
            );
        }

    }
}

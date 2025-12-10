
using AutoMetricsService.Domain.Entities;
using Core.Framework.Domain.Common;

namespace AutoMetricsService.Domain.Events
{
    public class SaleCreatedEvent : BaseEvent
    {
        public SaleCreatedEvent(Sale item)
        {
            Item = item;
        }

        public Sale Item { get; }

    }
}

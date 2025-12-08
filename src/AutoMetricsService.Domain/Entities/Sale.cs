using Core.Framework.Domain.Common;
using System;

namespace AutoMetricsService.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public int CenterId { get; set; }
        public Center Center { get; set; } = null!;
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;
        public int Units { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalTax { get; set; }
        public decimal Total { get; set; }
        public DateTimeOffset Date { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}

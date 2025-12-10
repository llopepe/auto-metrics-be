using Core.Framework.Domain.Common;
using System.Collections.Generic;

namespace AutoMetricsService.Domain.Entities
{
    public class Car : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ICollection<CarTax> Taxes { get; set; } = new List<CarTax>();
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}

using Core.Framework.Domain.Common;

namespace AutoMetricsService.Domain.Entities
{
    public class CarTax : BaseEntity
    {
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public decimal Percentage { get; set; }

    }
}

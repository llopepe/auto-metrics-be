using Core.Framework.Domain.Common;
using System.Collections.Generic;

namespace AutoMetricsService.Domain.Entities
{
    public class Center : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<Sale> Sales { get; set; }
    }
}

using Core.Framework.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMetricsService.Domain.Entities
{
    public class Sale: BaseEntity
    {
        public int CenterId { get; set; }
        public int CarId { get; set; }
        public int Units { get; set; } 
        public decimal UnitPrice { get; set; } 
        public decimal Total { get; set; }
        public DateTimeOffset Date { get; set; }
        public Center Center { get; set; } = null!;
        public Car Car { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMetricsService.Domain.EntitiesCustom
{
    public class SaleAmountResultCustom
    {
        public decimal UnitPrice { get; set; }
        public decimal TotalTax { get; set; }
        public decimal Total { get; set; }
    }
}

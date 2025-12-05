using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMetricsService.Domain.EntitiesCustom
{
    public class SalesVolumeCenterCustom
    {
        public int CenterId { get; set; }
        public string CenterName { get; set; }
        public decimal SalesVolume { get; set; }
    }
}

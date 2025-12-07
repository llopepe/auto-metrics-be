using AutoMetricsService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMetricsService.Application.Sales.Dto
{
    public class SaleDto
    {
        public int Id { get; set; }
        public int CenterId { get; set; }
        public string CenterName { get; set; } = string.Empty;
        public int CarId { get; set; }
        public string CarModel { get; set; } = string.Empty;
        public int Units { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalTax { get; set; }
        public decimal Total { get; set; }
        public DateTimeOffset Date { get; set; } 
        public string UserName { get; set; } = string.Empty;
    }
}

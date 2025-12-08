using AutoMetricsService.Domain.EntitiesCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UnitTests.EntitiesCustom
{
    [TestFixture]
    public class SaleAmountResultCustomTests
    {
        [Test]
        public void Constructor_ShouldInitializeProperties()
        {
            var dto = new SaleAmountResultCustom
            {
                UnitPrice = 100m,
                TotalTax = 21m,
                Total = 121m
            };

            Assert.AreEqual(100m, dto.UnitPrice);
            Assert.AreEqual(21m, dto.TotalTax);
            Assert.AreEqual(121m, dto.Total);
        }

        [Test]
        public void ShouldAllowUpdatingProperties()
        {
            var dto = new SaleAmountResultCustom
            {
                UnitPrice = 50m,
                TotalTax = 10m,
                Total = 60m
            };

            dto.UnitPrice = 75m;
            dto.TotalTax = 15m;
            dto.Total = 90m;

            Assert.AreEqual(75m, dto.UnitPrice);
            Assert.AreEqual(15m, dto.TotalTax);
            Assert.AreEqual(90m, dto.Total);
        }
    }
}

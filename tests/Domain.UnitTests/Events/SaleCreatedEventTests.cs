using AutoMetricsService.Domain.Entities;
using AutoMetricsService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UnitTests.Events
{
    [TestFixture]
    public class SaleCreatedEventTests
    {
        [Test]
        public void Constructor_ShouldAssignItem()
        {

            var sale = new Sale
            {
                Id = 1,
                Units = 3,
                UnitPrice = 100m,
                TotalTax = 21m,
                Total = 321m,
                UserName = "Leonardo"
            };

            var evt = new SaleCreatedEvent(sale);


            Assert.IsNotNull(evt.Item);
            Assert.AreEqual(sale, evt.Item);
            Assert.AreEqual(1, evt.Item.Id);
            Assert.AreEqual("Leonardo", evt.Item.UserName);
        }
    }
}

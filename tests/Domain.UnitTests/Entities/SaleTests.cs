using AutoMetricsService.Domain.Entities;
using Core.Framework.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UnitTests.Entities
{
    [TestFixture]
    public class SaleTests
    {
        [Test]
        public void Constructor_ShouldInitializeProperties()
        {
            var sale = new Sale();

            Assert.AreEqual(0, sale.CenterId);
            Assert.AreEqual(0, sale.CarId);
            Assert.AreEqual(0, sale.Units);
            Assert.AreEqual(0m, sale.UnitPrice);
            Assert.AreEqual(0m, sale.TotalTax);
            Assert.AreEqual(0m, sale.Total);
            Assert.AreEqual(string.Empty, sale.UserName);
            Assert.AreEqual(default(DateTimeOffset), sale.Date);
        }

        [Test]
        public void ShouldSetCarAndCarId()
        {
            var car = new Car { Id = 10, Name = "Toyota", Price = 1000m };
            var sale = new Sale { Car = car, CarId = car.Id };

            Assert.AreEqual(car, sale.Car);
            Assert.AreEqual(10, sale.CarId);
        }

        [Test]
        public void ShouldSetCenterAndCenterId()
        {
            var center = new Center { Id = 5, Name = "Sucursal 1" };
            var sale = new Sale { Center = center, CenterId = center.Id };

            Assert.AreEqual(center, sale.Center);
            Assert.AreEqual(5, sale.CenterId);
        }

        [Test]
        public void ShouldSetFinancialProperties()
        {
            var sale = new Sale
            {
                Units = 3,
                UnitPrice = 100m,
                TotalTax = 21m,
                Total = 321m
            };

            Assert.AreEqual(3, sale.Units);
            Assert.AreEqual(100m, sale.UnitPrice);
            Assert.AreEqual(21m, sale.TotalTax);
            Assert.AreEqual(321m, sale.Total);
        }

        [Test]
        public void ShouldSetDateAndUserName()
        {
            var now = DateTimeOffset.Now;
            var sale = new Sale { Date = now, UserName = "Leonardo" };

            Assert.AreEqual(now, sale.Date);
            Assert.AreEqual("Leonardo", sale.UserName);
        }

        [Test]
        public void ShouldInheritIdFromBaseEntity()
        {
            var sale = new Sale { Id = 42 };
            Assert.AreEqual(42, sale.Id);
        }

        [Test]
        public void CanAddDomainEvent_FromBaseEntity()
        {
            var sale = new Sale();
            var evt = new TestEvent();

            sale.AddDomainEvent(evt);

            Assert.AreEqual(1, sale.DomainEvents.Count);
            Assert.Contains(evt, sale.DomainEvents.ToList());
        }

        private class TestEvent : BaseEvent { }
    }
}

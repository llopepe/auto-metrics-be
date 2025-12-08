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
    public class CarTaxTests
    {
        [Test]
        public void Constructor_ShouldInitializeProperties()
        {
            var car = new Car { Name = "Toyota", Price = 1000m };
            var tax = new CarTax { Car = car, CarId = car.Id };

            Assert.IsNotNull(tax.Car);
            Assert.AreEqual(car, tax.Car);
            Assert.AreEqual(car.Id, tax.CarId);
            Assert.AreEqual(string.Empty, tax.Name);
            Assert.AreEqual(0m, tax.Percentage);
        }

        [Test]
        public void ShouldSetName()
        {
            var tax = new CarTax();
            tax.Name = "IVA";
            Assert.AreEqual("IVA", tax.Name);
        }

        [Test]
        public void ShouldSetPercentage()
        {
            var tax = new CarTax();
            tax.Percentage = 21.5m;
            Assert.AreEqual(21.5m, tax.Percentage);
        }

        [Test]
        public void ShouldSetCarAndCarId()
        {
            var car = new Car { Id = 5, Name = "Ford", Price = 5000m };
            var tax = new CarTax { Car = car, CarId = car.Id };

            Assert.AreEqual(car, tax.Car);
            Assert.AreEqual(5, tax.CarId);
        }

        [Test]
        public void ShouldInheritIdFromBaseEntity()
        {
            var tax = new CarTax { Id = 10 };
            Assert.AreEqual(10, tax.Id);
        }

        [Test]
        public void CanAddDomainEvent_FromBaseEntity()
        {
            var tax = new CarTax();
            var evt = new TestEvent();

            tax.AddDomainEvent(evt);

            Assert.AreEqual(1, tax.DomainEvents.Count);
            Assert.Contains(evt, tax.DomainEvents.ToList());
        }

        private class TestEvent : BaseEvent { }
    }
}

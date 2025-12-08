using AutoMetricsService.Domain.Entities;
using Core.Framework.Domain.Common;

namespace Domain.UnitTests.Entities
{
    [TestFixture]
    public class CarTests
    {
        [Test]
        public void Constructor_ShouldInitializeCollections()
        {
            var car = new Car();

            Assert.IsNotNull(car.Taxes);
            Assert.IsNotNull(car.Sales);
            Assert.AreEqual(0, car.Taxes.Count);
            Assert.AreEqual(0, car.Sales.Count);
        }

        [Test]
        public void ShouldSetName()
        {
            var car = new Car();
            car.Name = "Toyota Corolla";
            Assert.AreEqual("Toyota Corolla", car.Name);
        }

        [Test]
        public void ShouldSetPrice()
        {
            var car = new Car { Price = 1234.56m };
            Assert.AreEqual(1234.56m, car.Price);
        }

        [Test]
        public void AddTax_ShouldAddItemToTaxes()
        {
            var car = new Car();
            var tax = new CarTax { Name = "IVA", Percentage = 21 };

            car.Taxes.Add(tax);

            Assert.AreEqual(1, car.Taxes.Count);
            Assert.Contains(tax, car.Taxes.ToList());
        }

        [Test]
        public void AddSale_ShouldAddItemToSales()
        {
            var car = new Car();
            var sale = new Sale { Id = 1 };

            car.Sales.Add(sale);

            Assert.AreEqual(1, car.Sales.Count);
            Assert.Contains(sale, car.Sales.ToList());
        }

        [Test]
        public void ShouldAllowUpdatingFields()
        {
            var car = new Car { Name = "Old", Price = 100 };

            car.Name = "New";
            car.Price = 200;

            Assert.Multiple(() =>
            {
                Assert.AreEqual("New", car.Name);
                Assert.AreEqual(200, car.Price);
            });
        }

        [Test]
        public void CanAddDomainEvent_FromBaseEntity()
        {
            var tax = new Car();
            var evt = new TestEvent();

            tax.AddDomainEvent(evt);

            Assert.AreEqual(1, tax.DomainEvents.Count);
            Assert.Contains(evt, tax.DomainEvents.ToList());
        }

        private class TestEvent : BaseEvent { }
    }
}
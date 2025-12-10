using AutoMetricsService.Domain.Entities;
using Core.Framework.Domain.Common;

namespace Domain.UnitTests.Entities
{
    [TestFixture]
    public class CenterTests
    {
        [Test]
        public void Constructor_ShouldInitializeSalesCollection()
        {
            var center = new Center();
            center.Sales = new List<Sale>();

            Assert.IsNotNull(center.Sales);
            Assert.AreEqual(0, center.Sales.Count);
        }

        [Test]
        public void ShouldSetName()
        {
            var center = new Center();
            center.Name = "Sucursal 1";

            Assert.AreEqual("Sucursal 1", center.Name);
        }

        [Test]
        public void ShouldInheritIdFromBaseEntity()
        {
            var center = new Center { Id = 42 };
            Assert.AreEqual(42, center.Id);
        }

        [Test]
        public void CanAddSale()
        {
            var center = new Center();
            var sale = new Sale { Id = 1 };
            center.Sales = new List<Sale>();

            center.Sales.Add(sale);

            Assert.AreEqual(1, center.Sales.Count);
            Assert.Contains(sale, center.Sales.ToList());
        }

        [Test]
        public void CanAddDomainEvent_FromBaseEntity()
        {
            var center = new Center();
            var evt = new TestEvent();

            center.AddDomainEvent(evt);

            Assert.AreEqual(1, center.DomainEvents.Count);
            Assert.Contains(evt, center.DomainEvents.ToList());
        }


        private class TestEvent : BaseEvent { }
    }
}

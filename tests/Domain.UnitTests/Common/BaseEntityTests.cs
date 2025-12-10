using Core.Framework.Domain.Common;

namespace Domain.UnitTests.Common
{
    public class BaseEntityTests
    {
        private class TestEntity : BaseEntity { }

        private class TestEvent : BaseEvent { }

        private TestEntity _entity = null!;

        [SetUp]
        public void Setup()
        {
            _entity = new TestEntity();
        }

        [Test]
        public void AddDomainEvent_ShouldAddEvent()
        {
            var ev = new TestEvent();

            _entity.AddDomainEvent(ev);

            Assert.AreEqual(1, _entity.DomainEvents.Count);
            Assert.AreSame(ev, _entity.DomainEvents.First());
        }

        [Test]
        public void RemoveDomainEvent_ShouldRemoveEvent()
        {
            var ev = new TestEvent();
            _entity.AddDomainEvent(ev);

            _entity.RemoveDomainEvent(ev);

            Assert.AreEqual(0, _entity.DomainEvents.Count);
        }

        [Test]
        public void ClearDomainEvents_ShouldRemoveAllEvents()
        {
            _entity.AddDomainEvent(new TestEvent());
            _entity.AddDomainEvent(new TestEvent());

            _entity.ClearDomainEvents();

            Assert.IsEmpty(_entity.DomainEvents);
        }

        [Test]
        public void DomainEvents_ShouldBeReadOnly()
        {
            _entity.AddDomainEvent(new TestEvent());

            var events = _entity.DomainEvents;

            Assert.IsInstanceOf<System.Collections.ObjectModel.ReadOnlyCollection<BaseEvent>>(events);
        }
    }
}

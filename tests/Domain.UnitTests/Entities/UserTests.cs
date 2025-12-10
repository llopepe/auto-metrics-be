using AutoMetricsService.Domain.Entities;
using Core.Framework.Domain.Common;

namespace Domain.UnitTests.Entities
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void Constructor_ShouldInitializeProperties()
        {

            var user = new User();

            Assert.AreEqual(string.Empty, user.Email);
            Assert.AreEqual(string.Empty, user.PasswordHash);
            Assert.AreEqual(string.Empty, user.FullName);
            Assert.IsTrue(user.IsActive);
            Assert.AreEqual("User", user.Roles);
        }

        [Test]
        public void ShouldSetEmail()
        {
            var user = new User { Email = "leo@example.com" };
            Assert.AreEqual("leo@example.com", user.Email);
        }

        [Test]
        public void ShouldSetPasswordHash()
        {
            var user = new User { PasswordHash = "hash123" };
            Assert.AreEqual("hash123", user.PasswordHash);
        }

        [Test]
        public void ShouldSetFullName()
        {
            var user = new User { FullName = "Leonardo Lopepe" };
            Assert.AreEqual("Leonardo Lopepe", user.FullName);
        }

        [Test]
        public void ShouldSetIsActive()
        {
            var user = new User { IsActive = false };
            Assert.IsFalse(user.IsActive);
        }

        [Test]
        public void ShouldSetRoles()
        {
            var user = new User { Roles = "Admin,User" };
            Assert.AreEqual("Admin,User", user.Roles);
        }

        [Test]
        public void ShouldInheritIdFromBaseEntity()
        {
            var user = new User { Id = 99 };
            Assert.AreEqual(99, user.Id);
        }

        [Test]
        public void CanAddDomainEvent_FromBaseEntity()
        {
            var user = new User();
            var evt = new TestEvent();

            user.AddDomainEvent(evt);

            Assert.AreEqual(1, user.DomainEvents.Count);
            Assert.Contains(evt, user.DomainEvents.ToList());
        }

        private class TestEvent : BaseEvent { }
    }
}

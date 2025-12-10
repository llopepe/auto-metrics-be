using AutoMetricsService.Domain.EntitiesCustom;

namespace Domain.UnitTests.EntitiesCustom
{
    [TestFixture]
    public class SalesVolumeCenterCustomTests
    {
        [Test]
        public void Constructor_ShouldInitializeProperties()
        {
            var dto = new SalesVolumeCenterCustom
            {
                CenterId = 1,
                CenterName = "Sucursal 1",
                SalesVolume = 1500.50m
            };

            Assert.AreEqual(1, dto.CenterId);
            Assert.AreEqual("Sucursal 1", dto.CenterName);
            Assert.AreEqual(1500.50m, dto.SalesVolume);
        }

        [Test]
        public void ShouldAllowUpdatingProperties()
        {
            var dto = new SalesVolumeCenterCustom
            {
                CenterId = 2,
                CenterName = "Old Center",
                SalesVolume = 1000m
            };

            dto.CenterId = 3;
            dto.CenterName = "New Center";
            dto.SalesVolume = 2500.75m;

            Assert.AreEqual(3, dto.CenterId);
            Assert.AreEqual("New Center", dto.CenterName);
            Assert.AreEqual(2500.75m, dto.SalesVolume);
        }
    }
}

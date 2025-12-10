using AutoMetricsService.Domain.EntitiesCustom;

namespace Domain.UnitTests.EntitiesCustom
{
    [TestFixture]
    public class PercentageGlobalCustomTests
    {
        [Test]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange & Act
            var dto = new PercentageGlobalCustom
            {
                CenterId = 1,
                CenterName = "Sucursal 1",
                CarId = 10,
                CarModel = "Toyota Corolla",
                UnitsSold = 5,
                PercentageOfGlobal = 12.5m
            };

            // Assert
            Assert.AreEqual(1, dto.CenterId);
            Assert.AreEqual("Sucursal 1", dto.CenterName);
            Assert.AreEqual(10, dto.CarId);
            Assert.AreEqual("Toyota Corolla", dto.CarModel);
            Assert.AreEqual(5, dto.UnitsSold);
            Assert.AreEqual(12.5m, dto.PercentageOfGlobal);
        }

        [Test]
        public void ShouldAllowUpdatingProperties()
        {
            var dto = new PercentageGlobalCustom
            {
                CenterId = 2,
                CenterName = "Old Center",
                CarId = 20,
                CarModel = "Old Model",
                UnitsSold = 3,
                PercentageOfGlobal = 5m
            };

            // Act
            dto.CenterId = 3;
            dto.CenterName = "New Center";
            dto.CarId = 30;
            dto.CarModel = "New Model";
            dto.UnitsSold = 7;
            dto.PercentageOfGlobal = 15m;

            // Assert
            Assert.AreEqual(3, dto.CenterId);
            Assert.AreEqual("New Center", dto.CenterName);
            Assert.AreEqual(30, dto.CarId);
            Assert.AreEqual("New Model", dto.CarModel);
            Assert.AreEqual(7, dto.UnitsSold);
            Assert.AreEqual(15m, dto.PercentageOfGlobal);
        }
    }
}

using AutoMetricsService.Application.Sales.Dto;

namespace Application.UnitTests.Sales.Dto
{
    [TestFixture]
    public class PercentageGlobalDtoTests
    {
        [Test]
        public void CanAssignProperties()
        {
            var dto = new PercentageGlobalDto
            {
                CenterId = 1,
                CenterName = "Center A",
                CarId = 10,
                CarModel = "Model X",
                UnitsSold = 100,
                PercentageOfGlobal = 25.5m
            };

            Assert.AreEqual(1, dto.CenterId);
            Assert.AreEqual("Center A", dto.CenterName);
            Assert.AreEqual(10, dto.CarId);
            Assert.AreEqual("Model X", dto.CarModel);
            Assert.AreEqual(100, dto.UnitsSold);
            Assert.AreEqual(25.5m, dto.PercentageOfGlobal);
        }
    }
}

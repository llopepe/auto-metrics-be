using AutoMetricsService.Application.Sales.Dto;

namespace Application.UnitTests.Sales.Dto
{
    [TestFixture]
    public class SalesVolumeCenterDtoTests
    {
        [Test]
        public void CanAssignProperties()
        {
            var dto = new SalesVolumeCenterDto
            {
                CenterId = 1,
                CenterName = "Center A",
                SalesVolume = 1500.75m
            };

            Assert.AreEqual(1, dto.CenterId);
            Assert.AreEqual("Center A", dto.CenterName);
            Assert.AreEqual(1500.75m, dto.SalesVolume);
        }
    }
}

using AutoMetricsService.Application.Sales.Dto;

namespace Application.UnitTests.Sales.Dto
{
    [TestFixture]
    public class TotalSalesVolumeDtoTests
    {
        [Test]
        public void CanAssignTotalVolumeProperty()
        {
            var dto = new TotalSalesVolumeDto
            {
                TotalVolume = 12345.67m
            };

            Assert.AreEqual(12345.67m, dto.TotalVolume);
        }
    }
}

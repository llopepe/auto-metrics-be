using AutoMetricsService.Application.Sales.Dto;

namespace Application.UnitTests.Sales.Dto
{
    [TestFixture]
    public class SaleDtoTests
    {
        [Test]
        public void CanAssignProperties()
        {
            var date = DateTimeOffset.UtcNow;

            var dto = new SaleDto
            {
                Id = 1,
                CenterId = 10,
                CenterName = "Center A",
                CarId = 100,
                CarModel = "Model X",
                Units = 5,
                UnitPrice = 200m,
                TotalTax = 50m,
                Total = 1050m,
                Date = date,
                UserName = "user@example.com"
            };

            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual(10, dto.CenterId);
            Assert.AreEqual("Center A", dto.CenterName);
            Assert.AreEqual(100, dto.CarId);
            Assert.AreEqual("Model X", dto.CarModel);
            Assert.AreEqual(5, dto.Units);
            Assert.AreEqual(200m, dto.UnitPrice);
            Assert.AreEqual(50m, dto.TotalTax);
            Assert.AreEqual(1050m, dto.Total);
            Assert.AreEqual(date, dto.Date);
            Assert.AreEqual("user@example.com", dto.UserName);
        }
    }
}

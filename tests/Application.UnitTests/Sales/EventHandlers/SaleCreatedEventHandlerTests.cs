using AutoMetricsService.Application.Sales.EventHandlers;
using AutoMetricsService.Domain.Entities;
using AutoMetricsService.Domain.Events;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Sales.EventHandlers
{
    [TestFixture]
    public class SaleCreatedEventHandlerTests
    {
        private Mock<ILogger<SaleCreatedEventHandler>> _loggerMock = null!;
        private SaleCreatedEventHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<SaleCreatedEventHandler>>();
            _handler = new SaleCreatedEventHandler(_loggerMock.Object);
        }

        [Test]
        public async Task Handle_ShouldLogInformation()
        {

            var sale = new Sale();
            var notification = new SaleCreatedEvent(sale);
            var cancellationToken = CancellationToken.None;

            await _handler.Handle(notification, cancellationToken);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("SaleCreatedEvent")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}

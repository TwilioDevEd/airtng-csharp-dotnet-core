using System.Threading.Tasks;
using AirTNG.Web.Domain.Reservations;
using AirTNG.Web.Models;
using Moq;
using Xunit;

namespace AirTNG.Web.Tests.Domain
{
    public class NotifierTest
    {
        [Fact]
        public async Task SendNotification()
        {
            var mockSender = new Mock<ITwilioMessageSender>();
            mockSender.Setup(s => s.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            
            var notifier = new Notifier(mockSender.Object);

            await notifier.SendNotificationAsync(CreateNotification());
            
            mockSender.Verify(s => s.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        private Notification CreateNotification()
        {
            return Notification.BuildGuestNotification(new Reservation()
            {
                VacationProperty = new VacationProperty() { }
            });
        }
    }
}
using System.Threading.Tasks;
using AirTNG.Web.Domain.Reservations;
using AirTNG.Web.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using Twilio.AspNet.Core;
using Xunit;

namespace AirTNG.Web.Tests.Controllers
{
    public class SmsControllerTest
    {

        private readonly SmsController _controller;
        
        private readonly Mock<IApplicationDbRepository> _mockApplicationDbRepository;
        private readonly Mock<INotifier> _mockNotifier;
        
        public SmsControllerTest()
        {
            var host = new IdentityUser() {Id = "bob-id"};
            _mockApplicationDbRepository = new Mock<IApplicationDbRepository>();
            _mockApplicationDbRepository.Setup(a => a.FindUserByPhoneNumberAsync(It.IsAny<string>()))
                .ReturnsAsync(host);
            _mockApplicationDbRepository.Setup(a => a.FindFirstPendingReservationByHostAsync(host.Id))
                .ReturnsAsync(new Reservation()
                {
                    Id = 1, VacationProperty = new VacationProperty(){}
                });
            _mockApplicationDbRepository.Setup((a => a.UpdateReservationAsync(It.IsAny<Reservation>())))
                .ReturnsAsync(0);
            
            _mockNotifier = new Mock<INotifier>();
            _mockNotifier.Setup(n => n.SendNotificationAsync(It.IsAny<Notification>()))
                .Returns(Task.FromResult(false));
            
            _controller = new SmsController(_mockApplicationDbRepository.Object, _mockNotifier.Object);
        }

        [Fact]
        public async Task HandleAction_ChangeStateAndSendReply()
        {
            var result = await _controller.Handle("1111", "reject");
            Assert.IsType<TwiMLResult>(result);
            
            _mockApplicationDbRepository
                .Verify(a => a.FindUserByPhoneNumberAsync(It.IsAny<string>()), Times.Once);
            
            _mockApplicationDbRepository
                .Verify(a => a.FindFirstPendingReservationByHostAsync(It.IsAny<string>()), Times.Once);
            
            _mockApplicationDbRepository
                .Verify(a => a.UpdateReservationAsync(It.IsAny<Reservation>()), Times.Once);
            
            _mockNotifier.Verify(n => n.SendNotificationAsync(It.IsAny<Notification>()), Times.Once);
        }
    }
}
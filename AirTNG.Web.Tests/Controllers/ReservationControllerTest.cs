using System.Security.Claims;
using AirTNG.Web.Domain.Reservations;
using AirTNG.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace AirTNG.Web.Tests.Controllers
{
    public class ReservationControllerTest
    {
        private readonly ReservationController _controller;
        
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IApplicationDbRepository> _mockApplicationDbRepository;
        private readonly Mock<INotifier> _mockNotifier;
        
        public ReservationControllerTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserRepository.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new IdentityUser() { Id = "bob-id" });

            _mockNotifier = new Mock<INotifier>();
            _mockNotifier.Setup(n => n.SendNotificationAsync(It.IsAny<Notification>()))
                .Returns(Task.FromResult(false));
            
            _mockApplicationDbRepository = new Mock<IApplicationDbRepository>();
            
            // Return an object if searched Id = 1
            _mockApplicationDbRepository.Setup(c => c.FindVacationPropertyFirstOrDefaultAsync(1))
                .ReturnsAsync(new VacationProperty() { Id = 1, UserId = "bob-id" });

            _mockApplicationDbRepository.Setup(c => c.FindReservationWithRelationsAsync(It.IsAny<int>()))
                .ReturnsAsync(new Reservation()
                {
                    Id = 1,
                    VacationProperty = new VacationProperty() { Id = 1, Description = ""},
                    VacationPropertyId = 1
                });

            // Return 0 when create called
            _mockApplicationDbRepository.Setup(c => c.CreateReservationAsync(It.IsAny<Reservation>()))
                .ReturnsAsync(0);

            // Initialize controller
            _controller = new ReservationController(
                _mockApplicationDbRepository.Object, _mockUserRepository.Object, _mockNotifier.Object)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext() { }}
            };
        }

        [Fact]
        public async Task CreateActionGet_NullPropertyThenReturnsNotFound()
        {
            var result = await _controller.Create(null);
            Assert.IsType<NotFoundResult>(result);
            _mockApplicationDbRepository
                .Verify(a => a.FindVacationPropertyFirstOrDefaultAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateActionGet_InvalidPropertyThenReturnsNotFound()
        {
            var result = await _controller.Create(0);
            Assert.IsType<NotFoundResult>(result);
            _mockApplicationDbRepository
                .Verify(a => a.FindVacationPropertyFirstOrDefaultAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task CreateActionGet_ValidPropertyThenOpenCreateForm()
        {
            var result = await _controller.Create(1);
            Assert.IsType<ViewResult>(result);
            
            _mockApplicationDbRepository
                .Verify(a => a.FindVacationPropertyFirstOrDefaultAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task CreateActionPost_VacationPropertyIdNotMatchesThenNotFound()
        {
            var result = await _controller.Create(1, new Reservation() { });
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateActionPost_SaveAndRedirect()
        {
            var result = await _controller.Create(1, new Reservation(){});
            Assert.IsType<NotFoundResult>(result);

            result = await _controller.Create(1, new Reservation(){ VacationPropertyId = 1 });
            Assert.IsType<RedirectToActionResult>(result);
            
            _mockApplicationDbRepository
                .Verify(a => a.CreateReservationAsync(It.IsAny<Reservation>()), Times.Once);
            
            _mockApplicationDbRepository
                .Verify(a => a.FindReservationWithRelationsAsync(It.IsAny<int>()), Times.Once);
            
            _mockNotifier.Verify(n => n.SendNotificationAsync(It.IsAny<Notification>()), Times.Once);
        }
        
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using AirTNG.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AirTNG.Web.Tests.Controllers
{
    public class VacationPropertyControllerTest
    {

        private readonly VacationPropertyController _controller;

        private readonly Mock<IUserRepository> _mockUserRepository;
        
        private readonly Mock<IApplicationDbRepository> _mockAppRepository;

        public VacationPropertyControllerTest()
        {
            var user = new ApplicationUser
            {
                Id="bob-id"
            };
            
            var properties = new List<VacationProperty>
            {
                new VacationProperty {Description = "bob's property"},
                new VacationProperty {Description = "john's property"},
            };
            
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserRepository.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);
            // Return list of properties when ListAll... called
            _mockAppRepository = new Mock<IApplicationDbRepository>();
            _mockAppRepository.Setup(c => c.ListAllVacationPropertyAsync())
                .ReturnsAsync(properties);
            // Return 0 when create is called
            _mockAppRepository.Setup(c => c.CreateVacationPropertyAsync(It.IsAny<VacationProperty>()))
                .ReturnsAsync(0);
            // Return an object if searched Id = 1
            _mockAppRepository.Setup(c => c.FindVacationPropertyFirstOrDefaultAsync(1))
                .ReturnsAsync(new VacationProperty(){ Id = 1, UserId = "bob-id" });
            // Return 0 when delete is called
            _mockAppRepository.Setup(c => c.DeleteVacationPropertyAsync(It.IsAny<VacationProperty>()))
                .ReturnsAsync(0);
            
            // Initialize controller
            _controller = new VacationPropertyController(_mockUserRepository.Object, _mockAppRepository.Object)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext() { }}
            };
        }

        [Fact]
        public async Task IndexAction_RenderTheDefaultViewTest()
        {
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<VacationProperty>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void CreateActionGet_ThenRenderCreateForm()
        {
            var result = _controller.Create();
            
            // Assert
            Assert.IsType<ViewResult>(result);
        }
        
        [Fact]
        public async Task CreateActionPost_ThenCreatePropertyAndRedirect()
        {
            var vacationProperty = new VacationProperty()
            {
                Description = "ana's property",
            };
            
            var result = await _controller.Create(vacationProperty);

            // Assert
            Assert.True(_controller.ModelState.IsValid);
            Assert.IsType<RedirectToActionResult>(result);
            
            _mockUserRepository
                .Verify(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()), Times.Once);
            _mockAppRepository
                .Verify(a => a.CreateVacationPropertyAsync(It.IsAny<VacationProperty>()), Times.Once);
        }

        [Fact]
        public async Task EditActionGet()
        {
            var result = await _controller.Edit(null);
            Assert.IsType<NotFoundResult>(result);

            result = await _controller.Edit(0);
            Assert.IsType<NotFoundResult>(result);
            
            result = await _controller.Edit(1);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal( "bob-id", viewResult.ViewData["UserId"]);
            
            _mockAppRepository
                .Verify(a => a.FindVacationPropertyFirstOrDefaultAsync(It.IsAny<int?>()), Times.Exactly(2));
        }

        [Fact]
        public async Task EditActionPost()
        {
            var result = await _controller.Edit(1, new VacationProperty(){ Id = 2 });
            Assert.IsType<NotFoundResult>(result);
            
            result = await _controller.Edit(2, new VacationProperty(){ Id = 2 });
            Assert.IsType<NotFoundResult>(result);
        
            result = await _controller.Edit(1, new VacationProperty(){ Id = 1 });
            Assert.IsType<RedirectToActionResult>(result);

            _mockAppRepository
                .Verify(a => a.FindVacationPropertyFirstOrDefaultAsync(It.IsAny<int?>()), Times.Exactly(2));
            
            _mockAppRepository
                .Verify(a => a.UpdateVacationPropertyAsync(It.IsAny<VacationProperty>()), Times.Once);
        }

        [Fact]
        public async Task DeleteActionGet()
        {
            var result = await _controller.Delete(null);
            Assert.IsType<NotFoundResult>(result);
            
            result = await _controller.Delete(0);
            Assert.IsType<NotFoundResult>(result);
            
            result = await _controller.Delete(1);
            Assert.IsType<ViewResult>(result);
        }
        
        [Fact]
        public async Task DeleteConfirmedActionPost()
        {
            var result = await _controller.DeleteConfirmed(0);
            Assert.IsType<NotFoundResult>(result);
            
            result = await _controller.DeleteConfirmed(1);
            Assert.IsType<RedirectToActionResult>(result);
        }
        
    }
}
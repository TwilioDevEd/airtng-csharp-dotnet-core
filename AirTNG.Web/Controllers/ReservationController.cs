using System;
using System.Threading.Tasks;
using AirTNG.Web.Data;
using AirTNG.Web.Domain.Reservations;
using Microsoft.AspNetCore.Mvc;
using AirTNG.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AirTNG.Web.Tests.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly IApplicationDbRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly INotifier _notifier;

        public ReservationController(
            IApplicationDbRepository applicationDbRepository,
            IUserRepository userRepository,
            INotifier notifier)
        {
            _repository = applicationDbRepository;
            _userRepository = userRepository;
            _notifier = notifier;
        }

        // GET: Reservation/Create/1
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var property = await _repository.FindVacationPropertyFirstOrDefaultAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            ViewData["VacationProperty"] = property; 
            return View();
        }

        // POST: Reservation/Create/1
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Message,VacationPropertyId")] Reservation reservation)
        {
            if (id != reservation.VacationPropertyId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserAsync(HttpContext.User);
                reservation.Status = ReservationStatus.Pending;
                reservation.Name = user.Name;
                reservation.PhoneNumber = user.PhoneNumber;
                reservation.CreatedAt = DateTime.Now;

                await _repository.CreateReservationAsync(reservation);
                var notification = Notification.BuildHostNotification(
                    await _repository.FindReservationWithRelationsAsync(reservation.Id));
                
                await _notifier.SendNotificationAsync(notification);
                
                return RedirectToAction("Index", "VacationProperty");
            }
            
            ViewData["VacationProperty"] = await _repository.FindVacationPropertyFirstOrDefaultAsync(
                reservation.VacationPropertyId);
            return View(reservation);
        }

    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using AirTNG.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AirTNG.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace AirTNG.Web.Tests.Controllers
{

    [Authorize]
    public class VacationPropertyController : Controller
    {
        private readonly IApplicationDbRepository _repository;
        
        private readonly IUserRepository _userRepository;

        public VacationPropertyController(
            IUserRepository userRepository,
            IApplicationDbRepository repository)
        {
            _userRepository = userRepository;
            _repository = repository;
        }

        // GET: VacationProperty
        public async Task<IActionResult> Index()
        {
            return View(await _repository.ListAllVacationPropertyAsync());
        }

        // GET: VacationProperty/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VacationProperty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,ImageUrl")] VacationProperty vacationProperty)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserAsync(HttpContext.User);
                vacationProperty.UserId = user.Id;
                vacationProperty.CreatedAt = DateTime.Now;

                await _repository.CreateVacationPropertyAsync(vacationProperty);
                return RedirectToAction(nameof(Index));
            }
            return View(vacationProperty);
        }

        // GET: VacationProperty/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationProperty = await _repository.FindVacationPropertyFirstOrDefaultAsync(id);
            if (vacationProperty == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = vacationProperty.UserId;
            return View(vacationProperty);
        }

        // POST: VacationProperty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,ImageUrl")] VacationProperty vacationPropertyUpdate)
        {
            if (id != vacationPropertyUpdate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var vacationProperty = await _repository.FindVacationPropertyFirstOrDefaultAsync(id);
               
                if (vacationProperty == null)
                {
                    return NotFound();
                }
                await _repository.UpdateVacationPropertyAsync(vacationProperty);
               
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = vacationPropertyUpdate.UserId;
            return View(vacationPropertyUpdate);
        }

        // GET: VacationProperty/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationProperty = await _repository.FindVacationPropertyFirstOrDefaultAsync(id);
            if (vacationProperty == null)
            {
                return NotFound();
            }

            return View(vacationProperty);
        }

        // POST: VacationProperty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacationProperty = await _repository.FindVacationPropertyFirstOrDefaultAsync(id);
            if (vacationProperty == null)
            {
                return NotFound();
            }
            await _repository.DeleteVacationPropertyAsync(vacationProperty);
            return RedirectToAction(nameof(Index));
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirTNG.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AirTNG.Web.Models
{
    
    public interface IApplicationDbRepository
    {
        Task<List<VacationProperty>> ListAllVacationPropertyAsync();
        Task<VacationProperty> FindVacationPropertyFirstOrDefaultAsync(int? id);
        Task<int> CreateVacationPropertyAsync(VacationProperty vacationProperty);
        Task<int> UpdateVacationPropertyAsync(VacationProperty vacationProperty);
        Task<int> DeleteVacationPropertyAsync(VacationProperty vacationProperty);
        
        Task<int> CreateReservationAsync(Reservation reservation);
        Task<Reservation> FindReservationWithRelationsAsync(int id);
        Task<Reservation> FindFirstPendingReservationByHostAsync(string hostId);
        Task<int> UpdateReservationAsync(Reservation reservation);
        
        Task<IdentityUser> FindUserByPhoneNumberAsync(string number);
    }

    public class ApplicationDbRepository : IApplicationDbRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationDbRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VacationProperty>> ListAllVacationPropertyAsync()
        {
            return await _context.VacationProperties.ToListAsync();
        }

        public async Task<VacationProperty> FindVacationPropertyFirstOrDefaultAsync(int? id)
        {
            return await _context.VacationProperties.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> CreateVacationPropertyAsync(VacationProperty vacationProperty)
        {
            _context.Add(vacationProperty);
            return await _context.SaveChangesAsync();
        }
        
        public async Task<int> UpdateVacationPropertyAsync(VacationProperty vacationProperty)
        {
            _context.Add(vacationProperty);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteVacationPropertyAsync(VacationProperty vacationProperty)
        {
            _context.VacationProperties.Remove(vacationProperty);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CreateReservationAsync(Reservation reservation)
        {
            _context.Add(reservation);
                                
            return await _context.SaveChangesAsync();
        }

        public async Task<Reservation> FindReservationWithRelationsAsync(int id)
        {
            return await _context.Reservations
                .Where(r => r.Id == id)
                .Include(r => r.VacationProperty)
                .FirstAsync();
        }

        public async Task<Reservation> FindFirstPendingReservationByHostAsync(string hostId)
        {
            return await _context.Reservations
                .Where(r => r.VacationProperty.UserId == hostId && r.Status == ReservationStatus.Pending)
                .Include(r => r.VacationProperty)
                .FirstAsync();
        }

        public async Task<int> UpdateReservationAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            return await _context.SaveChangesAsync();
        }
        
        
        public async Task<IdentityUser> FindUserByPhoneNumberAsync(string number)
        {
            return await _context.Users.FirstAsync(u => u.PhoneNumber == number);
        }

    }
}
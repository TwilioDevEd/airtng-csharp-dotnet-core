using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirTNG.Web.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public ReservationStatus Status { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public int VacationPropertyId { get; set; }
        
        [ForeignKey("VacationPropertyId")]
        public VacationProperty VacationProperty { get; set; }
    }
    
    public enum ReservationStatus
    {
        Pending = 0,
        Confirmed = 1,
        Rejected = 2
    }
}
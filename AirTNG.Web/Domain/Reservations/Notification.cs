using System;
using System.Text;
using AirTNG.Web.Domain.Twilio;
using AirTNG.Web.Models;

namespace AirTNG.Web.Domain.Reservations
{
    public class Notification
    {
        public string To { get; set; }
        public string Message { get; set; }
        
        
        public static Notification BuildHostNotification(Reservation reservation)
        {
            var message = new StringBuilder();
            message.AppendFormat("You have a new reservation request from {0} for {1}:{2}",
                reservation.Name,
                reservation.VacationProperty.Description,
                Environment.NewLine);
            message.AppendFormat("{0}{1}",
                reservation.Message,
                Environment.NewLine);
            message.Append("Reply [accept] or [reject]");

            return new Notification
            {
                To = reservation.PhoneNumber,
                Message = message.ToString()
            };
        }
        
        public static Notification BuildGuestNotification(Reservation reservation)
        {
            var message = new StringBuilder();
            message.AppendFormat("Your reservation request to stay at {0} was {1} by the host.",
                reservation.VacationProperty.Description,
                reservation.Status == ReservationStatus.Confirmed ? "ACCEPTED" : "REJECTED" );

            return new Notification
            {
                To = reservation.PhoneNumber,
                Message = message.ToString()
            };
        }
    }
}
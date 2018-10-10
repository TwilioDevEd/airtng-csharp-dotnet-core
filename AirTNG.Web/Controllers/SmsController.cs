using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirTNG.Web.Data;
using AirTNG.Web.Domain.Reservations;
using AirTNG.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace AirTNG.Web.Tests.Controllers
{
    public class SmsController: TwilioController
    {
        private readonly IApplicationDbRepository _repository;
        private readonly INotifier _notifier;

        public SmsController(
            IApplicationDbRepository repository,
            INotifier notifier)
        {
            _repository = repository;
            _notifier = notifier;
        }
        

        // POST Sms/Handle
        [HttpPost]
        [AllowAnonymous]
        public async Task<TwiMLResult> Handle(string from, string body)
        {
            string smsResponse;
            
            try
            {
                var host = await _repository.FindUserByPhoneNumberAsync(from);
                var reservation = await _repository.FindFirstPendingReservationByHostAsync(host.Id);

                var smsRequest = body;
                reservation.Status =
                    smsRequest.Equals("accept", StringComparison.InvariantCultureIgnoreCase) ||
                    smsRequest.Equals("yes", StringComparison.InvariantCultureIgnoreCase)
                        ? ReservationStatus.Confirmed
                        : ReservationStatus.Rejected;

                await _repository.UpdateReservationAsync(reservation);
                smsResponse = $"You have successfully {reservation.Status} the reservation";

                // Notify guest with host response
                var notification = Notification.BuildGuestNotification(reservation);

                await _notifier.SendNotificationAsync(notification);
            }
            catch (InvalidOperationException)
            {
                smsResponse = "Sorry, it looks like you don't have any reservations to respond to.";
            }
            catch (Exception)
            {
                smsResponse = "Sorry, it looks like we get an error. Try later!";
            }
            
            return TwiML(Respond(smsResponse));
        }

        private static MessagingResponse Respond(string message)
        {
            var response = new MessagingResponse();
            response.Message(message);

            return response;
        }
    }
}
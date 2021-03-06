using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTNG.Web.Domain.Twilio;
using AirTNG.Web.Models;
using Twilio;
using Twilio.Clients;
using Twilio.TwiML.Messaging;

namespace AirTNG.Web.Domain.Reservations
{
    public interface INotifier
    {
        Task SendNotificationAsync(Notification notification);
    }

    public class Notifier : INotifier
    {
        private readonly ITwilioMessageSender _client;

        public Notifier(TwilioConfiguration configuration) : this(
            new TwilioMessageSender(configuration)
        ) { }

        public Notifier(ITwilioMessageSender client)
        {
            _client = client;
        }

        public async Task SendNotificationAsync(Notification notification)
        {
            await _client.SendMessageAsync(notification.To, notification.Message);
        }
    }
}

﻿using System.Threading.Tasks;
using AirTNG.Web.Domain.Twilio;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AirTNG.Web.Domain.Reservations
{
    public interface ITwilioMessageSender
    {
        Task SendMessageAsync(string to, string body);
    }
    
    public class TwilioMessageSender : ITwilioMessageSender
    {

        private readonly TwilioConfiguration _configuration;
        
        public TwilioMessageSender(TwilioConfiguration configuration)
        {
            _configuration = configuration;
            
            TwilioClient.Init(_configuration.AccountSid, _configuration.AuthToken);
        }

        public async Task SendMessageAsync(string to, string body)
        {
            await MessageResource.CreateAsync(new PhoneNumber(to),
                                              from: new PhoneNumber(_configuration.PhoneNumber),
                                              body: body);
        }
    }
}

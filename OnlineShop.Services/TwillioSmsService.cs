using OnlineShop.DTO;
using OnlineShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace OnlineShop.Services
{
    public class TwillioSmsService : ISmsService
    {

        const string accountSid = "AC3fbf63191267f4b50067a585825e4294";
        const string authToken = "4ade5aaa6abfc741eeb674bb70bd9ea5";

        public Task<SmsServiceResponseDTO> SendVerificationCode(string phoneNumber, string code)
        {
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Ваш код - " + code,
                from: new Twilio.Types.PhoneNumber("+15017122661"),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );

            return Task.FromResult(new SmsServiceResponseDTO { 
                StatusCode = 200,
                Message = "Сообщение успешно отправлено"
            });
        }
    }
}

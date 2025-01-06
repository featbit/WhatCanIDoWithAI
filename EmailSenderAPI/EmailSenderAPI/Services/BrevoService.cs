
using EmailSenderAPI.Models;
using Newtonsoft.Json;

namespace EmailSenderAPI.Services
{
    public class BrevoService : IBrevoService
    {
        public async Task<string> SendEmail(EmailPayload emailPayload)
        {
            await Task.Delay(1000);
            string jsonPayload = JsonConvert.SerializeObject(emailPayload);
            return jsonPayload;
        }
    }
}

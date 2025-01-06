using EmailSenderAPI.Models;

namespace EmailSenderAPI.Services
{
    public interface IBrevoService
    {
        Task<string> SendEmail(EmailPayload emailPayload);
    }
}

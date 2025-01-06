using EmailSenderAPI.Models;
using EmailSenderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailSenderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSenderController : ControllerBase
    {
        private IBrevoService _brevoService;

        public EmailSenderController(IBrevoService brevoService)
        {
            _brevoService = brevoService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailPayload emailPayload)
        {
            string rv = await _brevoService.SendEmail(emailPayload);
            return Ok(rv);
        }
    }
}

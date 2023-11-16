using BACK_SENDEMAIL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BACK_SENDEMAIL.Controllers
{

    [ApiController]
    [Route("api/email")]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromForm] IEmailBody request)
        {

            string result = _emailService.SendEmail(request);

            if (result == null)
            {
                return Ok("Email enviado com sucesso!");
            }
            else
            {
                return StatusCode(500, result);
            }
        }
    }
}

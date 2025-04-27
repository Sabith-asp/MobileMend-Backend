using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailServiceController : ControllerBase
    {
        private readonly IEmailService emailService;
        public EmailServiceController(IEmailService _emailService) {
            emailService = _emailService;
        }
        [Authorize(Roles = "User")]

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var response=await emailService.ConfirmEmail(email, token);
            return StatusCode(response.StatusCode, response);
        }

    }
}

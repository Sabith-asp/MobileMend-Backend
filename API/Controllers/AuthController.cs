using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Services;

namespace MobileMend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authservice;
        public AuthController(IAuthService _authservice)
        {
            authservice = _authservice;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO regdata)
        {

            var response = await authservice.Register(regdata);
            return StatusCode(response.StatusCode, response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO logindata)
        {

            var response = await authservice.Login(logindata);
            return StatusCode(response.StatusCode, response);

        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshAccessToken(Guid userid)
        {

            var response = await authservice.RefreshAccessToken(userid);
            return StatusCode(response.StatusCode, response);

        }
    }
}

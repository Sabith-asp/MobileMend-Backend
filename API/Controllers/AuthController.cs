using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Services;
using API.Controllers.Base;
using Application.Interfaces.Services;

namespace MobileMend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService authservice;
        public AuthController(IAuthService _authservice,IEmailService _emailService)
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

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append("accessToken", "", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });

            return Ok(new { message = "Logged out successfully" });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            if (role == "Technician") {
                var technicianId = await authservice.GetTechnicianIdByUserId(UserId);
                var technicianStatus = await authservice.GetTechnicianStatus(technicianId);
                return Ok(new
                {
                    Name = name,
                    Role = role,
                    TechnicianId= technicianId,
                    Status= technicianStatus
                });
            }
            return Ok(new
            {
                Name = name,
                Role = role
            });
        }

    }
}

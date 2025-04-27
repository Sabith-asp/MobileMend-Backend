using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        public AdminController(IAdminService _adminService) {
            adminService = _adminService;
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("dashboard-data")]
        public async Task<IActionResult> GetDashboardData() { 
        
            var response=await adminService.GetDashboardData();
            return StatusCode(response.StatusCode, response);
        }

    }
}

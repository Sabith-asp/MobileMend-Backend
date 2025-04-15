using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpareController : ControllerBase
    {
        private readonly ISpareService spareService;
        public SpareController(ISpareService _spareService)
        {
            spareService = _spareService;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddSpare(SpareCreateDTO newspare, Guid TechnicianID)
        { 
            var response=await spareService.AddSpare(newspare,  TechnicianID);
            return StatusCode(response.StatusCode, response);
        }
    }
}

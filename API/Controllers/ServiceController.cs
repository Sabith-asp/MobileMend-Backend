using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Repositories;
using MobileMend.Application.Interfaces.Services;

namespace MobileMend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService service;
        public ServiceController(IServiceService _service)
        {
            service = _service;
        }

        [HttpGet("all-service")]
        public async Task<IActionResult> GetAllService(bool isAdmin) {
        var response=await service.GetAllService(isAdmin);
            return StatusCode(response.StatusCode, response);
        }   



        [HttpPost("add-service")]
        public async Task<IActionResult> AddService(ServiceCreateDTO newservice)
        {
            var response = await service.AddService(newservice);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut("update-service/{serviceid}")]
        public async Task<IActionResult> UpdateService(Guid serviceid,[FromBody]ServiceCreateDTO servicedata) { 
        var response= await service.UpdateService(serviceid, servicedata);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("delete-service/{serviceid}")]

        public async Task<IActionResult> DeleteService(Guid serviceid) { 
        
            var response=await service.DeleteService(serviceid);    
            return StatusCode(response.StatusCode, response);
        }


    }
}



using API.Controllers.Base;
using Application.DTOs;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Repositories;
using MobileMend.Application.Interfaces.Services;

namespace MobileMend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : BaseController
    {
        private readonly IServiceService service;
        public ServiceController(IServiceService _service)
        {
            service = _service;
        }
        [HttpGet("get-service")]
        public async Task<IActionResult> GetService(string? search, Guid? serviceId)
        {       
            var filter = new ServiceFilterDTO
            {
                ServiceId = serviceId,
                Search = search
            };

            var response = await service.GetService(filter);
            return StatusCode(response.StatusCode, response);
        }


        [Authorize(Roles = "Admin")]

        [HttpPost("add-service")]
        public async Task<IActionResult> AddService(ServiceCreateDTO newservice)
        {
            var response = await service.AddService(newservice);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update-service")]
        public async Task<IActionResult> UpdateService([FromBody]ServiceCreateDTO servicedata) { 
        var response= await service.UpdateService( servicedata);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-service")]

        public async Task<IActionResult> DeleteService(Guid serviceid) { 
        
            var response=await service.DeleteService(serviceid);    
            return StatusCode(response.StatusCode, response);
        }


    }
}



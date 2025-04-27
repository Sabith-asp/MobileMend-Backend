using API.Controllers.Base;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Services;

namespace MobileMend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : BaseController
    {
        private readonly IDeviceService deviceService;
        public DeviceController(IDeviceService _deviceservice)
        {
            deviceService = _deviceservice;
        }
        [HttpGet("get-device")]
        public async Task<IActionResult> GetDevice(string? search, Guid? deviceId)
        {
            var filter = new DeviceFilterDTO {Search=search,DeviceId=deviceId};
            bool isAdmin = false;
            if (Role == "Admin") { 
                isAdmin = true;
            }
            var response = await deviceService.GetDevice(isAdmin, filter);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost("add-device")]
        public async Task<IActionResult> AddDevice(DeviceCreateDTO newdevice)
        {
            var response= await deviceService.AddDevice(newdevice);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("update-device")]
        public async Task<IActionResult> UpdateDevice(DeviceCreateDTO newdevice)
        {
            var response = await deviceService.UpdateDevice(newdevice.deviceid, newdevice);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin")]

        [HttpDelete("delete-device")]
        public async Task<IActionResult> DeleteDevice(Guid deviceid)
        {
            var response = await deviceService.DeleteDevice(deviceid);
            return StatusCode(response.StatusCode, response);
        }

    }
}

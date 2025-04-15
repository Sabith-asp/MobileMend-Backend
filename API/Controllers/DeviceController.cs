using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Services;

namespace MobileMend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService deviceService;
        public DeviceController(IDeviceService _deviceservice)
        {
            deviceService = _deviceservice;
        }

        [HttpGet("all-device")]
        public async Task<IActionResult> GetAllDevice(bool isAdmin)
        {
            var response = await deviceService.GetAllDevice(isAdmin);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("add-device")]
        public async Task<IActionResult> AddDevice(DeviceCreateDTO newdevice)
        {
            var response= await deviceService.AddDevice(newdevice);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("update-device/{deviceid}")]
        public async Task<IActionResult> UpdateDevice(Guid deviceid,DeviceCreateDTO newdevice)
        {
            var response = await deviceService.UpdateDevice(deviceid, newdevice);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("delete-device/{deviceid}")]
        public async Task<IActionResult> DeleteDevice(Guid deviceid)
        {
            var response = await deviceService.DeleteDevice(deviceid);
            return StatusCode(response.StatusCode, response);
        }

    }
}

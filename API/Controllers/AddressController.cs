using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Services;
using MobileMend.Application.Services;

namespace MobileMend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService addressService;
        public AddressController(IAddressService _addressService) {
            addressService = _addressService;
        }
        [HttpPost("add-address")]
        public async Task<IActionResult> AddAddress(Guid userid, AddressCreateDTO newaddress)
        {
            var response = await addressService.AddAddress(userid, newaddress);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("remove-address/{addressid}")]
        public async Task<IActionResult> RemoveAddress(Guid addressid)
        {
            var response = await addressService.RemoveAddress(addressid);
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("get-address")]
        public async Task<IActionResult> GetAddress(Guid userid)
        {
            var response = await addressService.GetAddress(userid);
            return StatusCode(response.StatusCode, response);
        }

    }
}

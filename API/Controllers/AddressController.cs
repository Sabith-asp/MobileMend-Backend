using API.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Services;
using MobileMend.Application.Services;

namespace MobileMend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : BaseController
    {
        private readonly IAddressService addressService;
        public AddressController(IAddressService _addressService) {
            addressService = _addressService;
        }
        [Authorize(Roles ="User")]
        [HttpPost("add-address")]
        public async Task<IActionResult> AddAddress(AddressCreateDTO newaddress)
        {
            var response = await addressService.AddAddress(UserId, newaddress);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "User")]
        [HttpDelete("remove-address/{addressid}")]
        public async Task<IActionResult> RemoveAddress(Guid addressid)
        {
            var response = await addressService.RemoveAddress(addressid);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "User")]
        [HttpGet("get-address")]
        public async Task<IActionResult> GetAddress()
        {
            var response = await addressService.GetAddress(UserId);
            return StatusCode(response.StatusCode, response);
        }

    }
}

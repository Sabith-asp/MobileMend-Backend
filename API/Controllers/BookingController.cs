using System.Security.Claims;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService bookingService;
        public BookingController(IBookingService _bookingService) {
        
            bookingService = _bookingService;
        }
        [HttpPost("book-service")]
        public async Task<IActionResult> BookService(Guid userId,BookingCreateDTO newbooking) {
        var response=await bookingService.BookService( userId,newbooking);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize]
        [HttpGet("get-booking")]
        public async Task<IActionResult> GetBooking(Guid? bookingId,string? status) {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response= await bookingService.GetBooking(userId, bookingId, status);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize]
        [HttpGet("secure-data")]
        public IActionResult SecureData()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(userId);
            return Ok($"Hello user {userId}");
        }


    }
}

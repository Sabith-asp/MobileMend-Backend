using System.Security.Claims;
using API.Controllers.Base;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : BaseController
    {
        private readonly IBookingService bookingService;
        public BookingController(IBookingService _bookingService) {
        
            bookingService = _bookingService;
        }
        [Authorize(Roles = "User")]
        [HttpPost("confirm-booking")]
        public async Task<IActionResult> BookService(BookingCreateDTO newbooking) {
        var response=await bookingService.BookService(UserId,newbooking);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin,Technician,User")]
        [HttpGet("get-booking")]
        public async Task<IActionResult> GetBooking(Guid? bookingId, ServiceStatus? status,Guid? technicianId,string? searchString) {
            var response= await bookingService.GetBooking(UserId,Role, bookingId, status, technicianId, searchString);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "User")]
        [HttpGet("get-booking-estimate")]
        public async Task<IActionResult> GetBookingEstimate(Guid technicianId,Guid addressId,Guid serviceId)
        {
            var response = await bookingService.GetBookingEstimate(technicianId, addressId, serviceId);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "User")]
        [HttpPatch("update-payment")]
        public async Task<IActionResult> UpdatePayment(Guid bookingId)
        {
            var response = await bookingService.UpdatePayment(bookingId);
            return StatusCode(response.StatusCode, response);
        }
    }
}

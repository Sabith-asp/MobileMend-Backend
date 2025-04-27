using API.Controllers.Base;
using Application.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Services;
using MobileMend.Domain.Entities;
using Mysqlx.Crud;

namespace MobileMend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicianController : BaseController
    {
        private readonly ITechnicianService technicianService;
        public TechnicianController(ITechnicianService _technicianService)
        {
            technicianService = _technicianService;
        }


        //user
        [Authorize(Roles = "User")]
        [HttpGet("get-best-technicians")]
        public async Task<IActionResult> GetBestTechnician(Guid customerAddressId,Guid deviceId)
        {
            var response = await technicianService.GetBestTechnician(customerAddressId, deviceId);
            return StatusCode(response.StatusCode, response);

        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("get-technicians")]
        public async Task<IActionResult> GetTechnicians(Guid? technicianId,string? search)
        {
            var filter = new TechnicianFilterDTO { technicianId = technicianId, Search = search };
            var response = await technicianService.GetTechnicians(filter);
            return StatusCode(response.StatusCode, response);

        }


        //technician
        [Authorize(Roles = "User")]
        [HttpPost("technician-request")]
        public async Task<IActionResult> TechnicianRequest([FromForm]TechnicianRequestCreateDTO newrequest)
        {
            var response = await technicianService.TechnicianRequest(UserId,newrequest);
            return StatusCode(response.StatusCode, response);
        }

        //admin
        [Authorize(Roles = "Admin")]
        [HttpPatch("update-request-status")]

        public async Task<IActionResult> UpdateRequestStatus(UpdateTechnicianRequestDTO update)
        {
            var response = await technicianService.UpdateRequestStatus(update.TechnicianRequestId, update.Status, update.AdminRemarks);
            return StatusCode(response.StatusCode, response);
        }

        //admin
        [Authorize(Roles = "Admin")]
        [HttpGet("get-requests")]
        public async Task<IActionResult> GetRequests(TechnicianRequestStatuses? status,string? search) {
            var response=await technicianService.GetRequests(status, search);
            return StatusCode(response.StatusCode, response);
        }

        //technician
        [Authorize(Roles = "Technician")]
        [HttpPatch("update-service-request")]
        public async Task<IActionResult> UpdateServiceRequest(UpdateServiceRequestDTO statusdata)
        {
            var response = await technicianService.UpdateServiceRequest(statusdata);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin,Technician")]
        [HttpPatch("update-service-status")]
        public async Task<IActionResult> UpdateServiceStatus(UpdateServiceStatusDTO updatedStatus)
        {
            var response = await technicianService.UpdateServiceStatus(updatedStatus.TechnicianId, updatedStatus.Status, updatedStatus.BookingId);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Technician")]
        [HttpPatch("update-availability")]

        public async Task<IActionResult> UpdateAvailability(UpdateAvailablityDTO status) {
            var response = await technicianService.UpdateAvailability(UserId, status);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "User")]
        [HttpGet("find-technician-auto")]
        public async Task<IActionResult> FindTechnician(Guid addressId, Guid deviceId) {

            var response = await technicianService.FindTechnician(addressId, deviceId);
            return StatusCode(response.StatusCode, response);

        }
        [Authorize(Roles = "Technician")]
        [HttpPatch("update-current-location")]
        public async Task<IActionResult> UpdateCurrentLocation(UpdateCurrentLocationDTO currentLocation) {
            var response = await technicianService.UpdateCurrentLocation(currentLocation);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("block-technician/{technicianId}")]
        public async Task<IActionResult> ToggleTechnicianStatus(Guid technicianId)
        {
            var response = await technicianService.ToggleTechnicianStatus(technicianId);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("remove-technician/{technicianId}")]
        public async Task<IActionResult> RemoveTechnician(Guid technicianId)
        {
            var response = await technicianService.RemoveTechnician(technicianId);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Technician")]
        [HttpGet("dashboard-data")]
        public async Task<IActionResult> GetTechnicianDashboardData()
        {
            var response = await technicianService.GetTechnicianDashboardData(UserId);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Technician,Admin")]
        [HttpPost("notify-spare-payment")]
        public async Task<IActionResult> NotifySparesPayment(Guid bookingId)
        {
            var response = await technicianService.NotifySparesPayment(bookingId);
            return StatusCode(response.StatusCode, response);
        }

    }
}

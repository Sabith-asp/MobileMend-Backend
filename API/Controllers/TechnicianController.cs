using Application.DTOs;
using Domain.Enums;
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
    public class TechnicianController : ControllerBase
    {
        private readonly ITechnicianService technicianService;
        public TechnicianController(ITechnicianService _technicianService)
        {
            technicianService = _technicianService;
        }
        //technician

        [HttpPost("technician-request")]
        public async Task<IActionResult> TechnicianRequest([FromForm]TechnicianRequestCreateDTO newrequest)
        {
            var response = await technicianService.TechnicianRequest(newrequest);
            return StatusCode(response.StatusCode, response);
        }

        //admin

        [HttpPatch("update-request-status")]

        public async Task<IActionResult> UpdateRequestStatus(Guid technicianRequestId,bool status, string adminRemarks)
        {
            var response = await technicianService.UpdateRequestStatus(technicianRequestId, status, adminRemarks);
            return StatusCode(response.StatusCode, response);
        }

        //admin

        [HttpGet("get-request-bystatus")]
        public async Task<IActionResult> GetRequestByStatus(TechnicianRequestStatuses status) {
            var response=await technicianService.GetRequestsByStatus(status);
            return StatusCode(response.StatusCode, response);
        }

        //technician
        [HttpPatch("update-service-request")]
        public async Task<IActionResult> UpdateServiceRequest(Guid technicianId,UpdateServiceRequestDTO statusdata)
        {
            var response = await technicianService.UpdateServiceRequest(technicianId, statusdata);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("update-service-status")]
        public async Task<IActionResult> UpdateServiceStatus(Guid technicianId, ServiceStatus status, Guid bookingId)
        {
            Console.WriteLine(status);
            var response = await technicianService.UpdateServiceStatus(technicianId,status,bookingId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("update-availability")]

        public async Task<IActionResult> UpdateAvailability(Guid technicianId, TechnicianAvailabilityStatus status) {
            var response = await technicianService.UpdateAvailability(technicianId, status);
            return StatusCode(response.StatusCode, response);
        }

    }
}

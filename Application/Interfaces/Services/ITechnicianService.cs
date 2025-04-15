
using Application.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;

namespace MobileMend.Application.Interfaces.Services
{
    public interface ITechnicianService
    {
        Task<ResponseDTO<object>> TechnicianRequest(TechnicianRequestCreateDTO newrequest);
        Task<ResponseDTO<object>> UpdateRequestStatus(Guid technicianRequestId,bool status, string adminRemarks);
        Task<ResponseDTO<IEnumerable<object>>> GetRequestsByStatus(TechnicianRequestStatuses status);
        Task<ResponseDTO<object>> UpdateServiceRequest(Guid TechnicianID,UpdateServiceRequestDTO statusdata);

        Task<ResponseDTO<object>> UpdateServiceStatus(Guid technicianId, ServiceStatus status, Guid bookingId);
        Task<ResponseDTO<object>> UpdateAvailability(Guid technicianId, TechnicianAvailabilityStatus status);
    }
}

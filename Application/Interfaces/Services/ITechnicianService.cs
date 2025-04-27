
using Application.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;

namespace MobileMend.Application.Interfaces.Services
{
    public interface ITechnicianService
    {
        Task<ResponseDTO<object>> TechnicianRequest(string userId, TechnicianRequestCreateDTO newrequest);
        Task<ResponseDTO<object>> UpdateRequestStatus(Guid technicianRequestId, bool status, string? adminRemarks);
        Task<ResponseDTO<IEnumerable<object>>> GetRequests(TechnicianRequestStatuses? status, string? search);
        Task<ResponseDTO<object>> UpdateServiceRequest(UpdateServiceRequestDTO statusdata);

        Task<ResponseDTO<object>> UpdateServiceStatus(Guid technicianId, ServiceStatus status, Guid bookingId);
        Task<ResponseDTO<object>> UpdateAvailability(string userid, UpdateAvailablityDTO status);

        Task<ResponseDTO<IEnumerable<TechnicianDTO>>> GetBestTechnician(Guid customerAddressId, Guid deviceId);

        Task<ResponseDTO<object>> FindTechnician(Guid addressId, Guid deviceId);

        Task<ResponseDTO<object>> UpdateCurrentLocation(UpdateCurrentLocationDTO currentLocation);

        Task<ResponseDTO<IEnumerable<GetTechnicianByAdminDTO>>> GetTechnicians(TechnicianFilterDTO filter);
        Task<ResponseDTO<string>> RemoveTechnician(Guid technicianId);
        Task<ResponseDTO<string>> ToggleTechnicianStatus(Guid technicianId);

        Task<ResponseDTO<TechnicianDashboardDataDTO>> GetTechnicianDashboardData(string userId);

        Task<ResponseDTO<object>> NotifySparesPayment(Guid bookingId);

    }
}

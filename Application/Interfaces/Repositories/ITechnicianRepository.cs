using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Domain.Entities;

namespace MobileMend.Application.Interfaces.Repositories
{
    public interface ITechnicianRepository
    {
        Task<int> TechnicianRequest(TechncianRequestAddDTO newrequest);
        Task<TechnicianRequest> CheckAlreadyRequested(string userid);

        Task<int> UpdateRequestStatus(Guid technicianRequestId,string status, string? adminRemarks);
        Task<IEnumerable<TechnicianRequest>> GetRequests(TechnicianRequestStatuses? status, string? search);

        Task<int> AddTechnician(Guid technicianRequestId);

        Task<int> UpdateServiceRequest(Guid TechnicianID,UpdateServiceRequestDTO statusdata,string technicianDecision);
        Task<Guid> GetUserIdByBookingID(Guid bookingid);

        Task<int> UpdateServiceStatus(Guid technicianId, ServiceStatus status, Guid bookingId);

        Task<int> UpdateAvailability(Guid technicianID, UpdateAvailablityDTO status);

        Task<IEnumerable<TechnicianDTO>> GetBestTechnician(Guid customerAddressId, Guid deviceId);
        Task<int> UpdateRoleToTechnician(Guid userid);

        Task<TechnicianAssignmentResult> FindTechnician(Guid addressID, Guid deviceID, IEnumerable<Guid>? alreadyAssigned = null);
        Task<Guid> GetTechnicianIdByUserId(string Userid);

        Task<int> UpdateCurrentLocation(UpdateCurrentLocationDTO currentLocation);

        Task<IEnumerable<GetTechnicianByAdminDTO>> GetTechnicians(TechnicianFilterDTO filter);

        Task<int> ToggleTechnicianStatus(Guid technicianId);
        Task<int> RemoveTechnician(Guid technicianId);

        Task<TechnicianDashboardDataDTO> GetTechnicianDashboardData(Guid technicianId);
        Task<IEnumerable<TechnicianRevenueChartDataDTO>> GetMonthlyRevenueAndBookings(Guid technicianId);

        Task<Guid> ReassignTechnician(Guid bookingId);
    }
}

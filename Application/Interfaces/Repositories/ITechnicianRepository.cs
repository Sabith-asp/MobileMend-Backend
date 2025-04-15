using Application.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Domain.Entities;

namespace MobileMend.Application.Interfaces.Repositories
{
    public interface ITechnicianRepository
    {
        Task<int> TechnicianRequest(TechncianRequestAddDTO newrequest);
        Task<TechnicianRequest> CheckAlreadyRequested(Guid userid);

        Task<int> UpdateRequestStatus(Guid technicianRequestId,string status, string adminRemarks);
        Task<IEnumerable<TechnicianRequest>> GetRequestsByStatus(TechnicianRequestStatuses status);

        Task<int> AddTechnician(Guid technicianRequestId);

        Task<int> UpdateServiceRequest(Guid TechnicianID,UpdateServiceRequestDTO statusdata,string technicianDecision);
        Task<Guid> GetUserIdByBookingID(Guid bookingid);

        Task<int> UpdateServiceStatus(Guid technicianId, ServiceStatus status, Guid bookingId);

        Task<int> UpdateAvailability(Guid technicianID, TechnicianAvailabilityStatus status);



    }
}

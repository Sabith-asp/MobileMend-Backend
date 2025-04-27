using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;

namespace Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<BookingResultDTO> BookService(string userId,double bookingCharge,BookingCreateDTO newbooking, double travelAllowance,double distance,Guid technicianid);
        Task<TechnicianAssignmentResult> FindTechnician(Guid addressID, Guid DeviceID);
        Task<TechnicianAssignmentResult> GetSelectedTechnicianInfo(Guid? technicianid, Guid addressid);

        Task<IEnumerable<GetBookingDetailsDTO>> GetAllBookings(string? searchString);

        Task<IEnumerable<GetBookingDetailsDTO>> GetBookingsByUserID(string? userId);

        Task<IEnumerable<GetBookingDetailsDTO>> GetBookingsByStatus(ServiceStatus? status,string? searchString);

        Task<GetBookingDetailsDTO> GetBookingByID(Guid? bookingId);
        Task<double> GetBookingDistance(Guid technicianId, Guid addressId);
        Task<double> GetServiceCharge(Guid serviceid);

        Task<IEnumerable<GetBookingDetailsDTO>> GetBookingsByTechnicianIdAndStatus(Guid? technicianId, ServiceStatus? status);
        Task<IEnumerable<GetBookingDetailsDTO>> GetBookingsInProgress(Guid? technicianId, ServiceStatus? status, string? searchString);

        Task<int> UpdatePayment(Guid bookingId);
    }
}

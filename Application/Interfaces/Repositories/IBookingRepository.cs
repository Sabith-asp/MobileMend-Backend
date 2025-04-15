using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;

namespace Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<int> BookService(double bookingCharge,BookingCreateDTO newbooking, double travelAllowance,double distance,Guid technicianid);
        Task<TechnicianAssignmentResult> FindTechnician(Guid addressID, Guid DeviceID);
        Task<TechnicianAssignmentResult> GetSelectedTechnicianInfo(Guid? technicianid, Guid addressid);

        Task<IEnumerable<GetBookingDetailsDTO>> GetAllBookings();

        Task<IEnumerable<GetBookingDetailsDTO>> GetBookingsByUserID(string? userId);

        Task<IEnumerable<GetBookingDetailsDTO>> GetBookingsByStatus(string status);

        Task<GetBookingDetailsDTO> GetBookingByID(Guid? bookingId);
    }
}

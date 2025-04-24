using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface IBookingService
    {
        Task<ResponseDTO<object>> BookService(string userId,BookingCreateDTO newbooking);
        Task<ResponseDTO<object>> GetBooking(string? userId,string? Role, Guid? bookingId, ServiceStatus? status, Guid? technicianId, string? searchString);
        Task<ResponseDTO<object>> GetBookingEstimate(Guid technicianId, Guid addressId,Guid serviceId);
    }
}

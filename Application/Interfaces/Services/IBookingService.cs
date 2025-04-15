using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface IBookingService
    {
        Task<ResponseDTO<object>> BookService(Guid userId,BookingCreateDTO newbooking);
        Task<ResponseDTO<object>> GetBooking(string? userId, Guid? bookingId, string? status);
    }
}

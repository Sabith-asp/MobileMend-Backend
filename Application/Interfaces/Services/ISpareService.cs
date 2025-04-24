using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MobileMend.Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface ISpareService
    {
        Task<ResponseDTO<object>> AddSpare(SpareCreateDTO newSpare, Guid TechnicianID);
        Task<ResponseDTO<object>> GetSpareByBookingId(Guid bookingId);
    }
}

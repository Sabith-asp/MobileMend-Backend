using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MobileMend.Application.DTOs;

namespace Application.Interfaces.Repositories
{
    public interface ISpareRepository
    {
        Task<int> AddSpare(SpareCreateDTO newSpare, Guid TechnicianID);
        Task<IEnumerable<GetSpareDTO>> GetSpareByBookingId(Guid bookingId);
    }
}

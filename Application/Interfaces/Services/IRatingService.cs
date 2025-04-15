using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MobileMend.Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface IRatingService
    {
        Task<ResponseDTO<object>> AddRating(RatingCreateDTO newrating);
    }
}

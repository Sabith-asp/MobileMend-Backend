using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MobileMend.Application.DTOs;

namespace Application.Interfaces.Repositories
{
    public interface IRatingRepository
    {
        Task<int> AddOrUpdateRating(RatingCreateDTO newrating);
        Task<int> HideReview(Guid reviewId);
        Task<string> GetStatus(Guid bookingId);
    }
}

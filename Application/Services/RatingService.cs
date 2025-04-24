using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;

namespace Application.Services
{
    public class RatingService:IRatingService
    {
        private readonly IRatingRepository ratingRepository;
        public RatingService(IRatingRepository _ratingRepository) {
            ratingRepository = _ratingRepository;
                }
        public async Task<ResponseDTO<object>> AddRating(RatingCreateDTO newrating) {
            try {
                var bookingStatus = await ratingRepository.GetStatus(newrating.BookingID);
                if (bookingStatus != "Completed") {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Service not completed for adding rating" };

                }
                var result=await ratingRepository.AddOrUpdateRating(newrating);
                if (result < 1) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in adding rating" };
                }
                return new ResponseDTO<object> { StatusCode = 200, Message = "rating added" };
            } catch(Exception ex) {
                return new ResponseDTO<object> { StatusCode = 500, Message =ex.Message };
            }
        }

        public async Task<ResponseDTO<object>> HideReview(Guid reviewId)

        {
            try
            {
                var result = await ratingRepository.HideReview(reviewId);
                if (result < 1)
                {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in adding service" };
                }
                return new ResponseDTO<object> { StatusCode = 200, Message = "rating removed" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Message = ex.Message };
            }
        }
    }
}

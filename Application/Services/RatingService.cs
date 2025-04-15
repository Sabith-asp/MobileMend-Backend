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
                var result=await ratingRepository.AddRating(newrating);
                if (result < 1) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in adding service" };
                }
                return new ResponseDTO<object> { StatusCode = 200, Message = "rating added" };
            } catch(Exception ex) {
                return new ResponseDTO<object> { StatusCode = 500, Message =ex.Message };
            }
        }
    }
}

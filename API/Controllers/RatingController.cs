using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService ratingService;
        public RatingController(IRatingService _ratingService) { ratingService = _ratingService; }

        [HttpPost("add")]
        public async Task<IActionResult> AddRating(RatingCreateDTO newrating) {
        
            var response=await ratingService.AddRating(newrating);
            return StatusCode(response.StatusCode, response);
        }
    }
}

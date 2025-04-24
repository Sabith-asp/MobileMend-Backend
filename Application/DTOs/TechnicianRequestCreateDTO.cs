using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MobileMend.Application.DTOs
{
    public class TechnicianRequestCreateDTO
    {
        [Required]
        public int Experience { get; set; }
        [Required]
        public IFormFile Resume { get; set; }
        [Required]
        public string Specialization { get; set; }
        [Required]
        public string Bio { get; set; }
        [Required]
        public string Place { get; set; }

        public decimal Longitude { get; set; } = 0;

        public decimal Latitude { get; set; } = 0;
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MobileMend.Application.DTOs
{
    public class TechnicianRequestCreateDTO
    {
        [Required]
        public Guid UserID { get; set; }
        [Required]
        public int Experience { get; set; }
        [Required]
        public IFormFile Resume { get; set; }
        [Required]
        public string Specialization { get; set; }
        [Required]
        public string Bio { get; set; }
        [Required]
        public decimal Longitude { get; set; }
        [Required]
        public decimal Latitude { get; set; }
    }
}

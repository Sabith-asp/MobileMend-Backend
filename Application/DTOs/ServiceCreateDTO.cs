using System.ComponentModel.DataAnnotations;

namespace MobileMend.Application.DTOs
{
    public class ServiceCreateDTO
    {
        [Required]
        public string ServiceName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int EstimatedTime { get; set; }
        [Required]
        public bool IsPopular { get; set; }
    }
}

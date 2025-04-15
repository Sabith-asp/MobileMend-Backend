using System.ComponentModel.DataAnnotations;

namespace MobileMend.Application.DTOs
{
    public class AddressCreateDTO
    {
        [Required]
        public string AddressDetail { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Pincode { get; set; }

        [Required]
        public string State { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public decimal Latitude { get; set; } =0;
        [Required]
        public decimal Longitude { get; set; } = 0;


    }
}

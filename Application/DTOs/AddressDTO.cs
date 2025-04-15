namespace MobileMend.Application.DTOs
{
    public class AddressDTO
    {
        public Guid AddressID { get; set; }
        public string AddressDetail { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }

        public string State { get; set; }
        public string Street { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

    }
}

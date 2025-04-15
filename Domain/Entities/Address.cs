namespace MobileMend.Domain.Entities
{
    public class Address
    {
        public Guid AddressID { get; set; }
        public Guid UserID { get; set; }
        public string AddressDetail { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }

        public string State { get; set; }
        public string Street { get; set; }
        public decimal Latitude { get; set; } 
        public decimal Longitude { get; set; }

        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}

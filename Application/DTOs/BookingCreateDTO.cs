namespace MobileMend.Application.DTOs
{
    public class BookingCreateDTO
    {
        public Guid AddressID { get; set; }
        public Guid DeviceID { get; set; }
        public Guid ServiceID { get; set; }
        public Guid TechnicianID { get; set; }
        public string Issue { get; set; }
    }
}

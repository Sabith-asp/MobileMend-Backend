namespace MobileMend.Application.DTOs
{
    public class BookingCreateDTO
    {
        public Guid CustomerID { get; set; }
        public Guid AddressID { get; set; }
        public Guid DeviceID { get; set; }
        public Guid ServiceID { get; set; }
        public Guid? TechnicianID { get; set; }
        public string Email { get; set; }
        public string CustomerName { get; set; }
        public string Issue { get; set; }
        public string Phone { get; set; }
    }
}

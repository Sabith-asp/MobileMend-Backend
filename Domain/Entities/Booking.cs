using Domain.Entities;

namespace MobileMend.Domain.Entities
{
    public class Booking
    {
        public Guid BookingID { get; set; }
        public Guid CustomerID { get; set; }
        public Guid AddressID { get; set; }
        public Guid DeviceID { get; set; }
        public Guid ServiceID { get; set; }
        public Guid TechnicianID { get; set; }
        public string BookingStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Email { get; set; }
        public string CustomerName { get; set; }
        public string Issue { get; set; }
        public string PaymentStatus { get; set; }
        public string Phone { get; set; }
        public DateTime CompletedAt { get; set; }
        public Guid UpdatedBy { get; set; }

    }
}

namespace MobileMend.Domain.Entities
{
    public class TechnicianRequest
    {
        public Guid TechnicianRequestID { get; set; }
        public Guid UserID { get; set; }
        public int Experience { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public byte[] DocumentData { get; set; }
        public string Specialization { get; set; }
        public string Bio { get; set; }
        public string Status { get; set; }
        public string Place { get; set; }
        public string AdminRemark { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public DateTime RequestDate { get; set; }
    }
}

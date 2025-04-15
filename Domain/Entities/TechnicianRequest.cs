namespace MobileMend.Domain.Entities
{
    public class TechnicianRequest
    {
        public Guid TechnicianRequestID { get; set; }
        public Guid UserID { get; set; }
        public int Experience { get; set; }
        public string ResumeUrl { get; set; }
        public string Specialization { get; set; }
        public string Bio { get; set; }
        public string Status { get; set; } 
        public string AdminRemark { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

    }
}

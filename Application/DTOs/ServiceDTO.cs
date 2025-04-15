namespace MobileMend.Application.DTOs
{
    public class ServiceDTO
    {
        public Guid ServiceID { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }
        public int EstimatedTime { get; set; }
        public bool IsPopular { get; set; }
    }
}
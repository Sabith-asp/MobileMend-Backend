namespace MobileMend.Domain.Entities
{
    public class Service
    {
        public Guid ServiceID { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int EstimatedTime { get; set; }
        public string Category { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPopular { get; set; }
        public bool IsDeleted { get; set; }

    }
}

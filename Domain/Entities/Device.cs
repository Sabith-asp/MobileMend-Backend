namespace MobileMend.Domain.Entities
{
    public class Device
    {
        public Guid DeviceID { get; set; }  
        public string DeviceName { get; set; }
        public string Brand { get; set; }
        public string DeviceType { get; set; }
        public string Model { get; set; }
        public int ReleaseYear { get; set; }
        public string CommonIssues { get; set; }
        public string RepairableComponents { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

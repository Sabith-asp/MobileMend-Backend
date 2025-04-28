
namespace MobileMend.Domain.Entities
{
    public class Document
    {
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public byte[] Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
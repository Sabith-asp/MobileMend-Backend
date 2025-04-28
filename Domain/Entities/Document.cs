using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Document
    {
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte[] Data { get; set; }
    }
}

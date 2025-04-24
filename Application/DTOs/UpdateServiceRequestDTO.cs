using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateServiceRequestDTO
    {
        public Guid TechnicianId { get; set; }
        public Guid BookingID { get; set; }
        public bool Status { get; set; }
        public string? RejectionReason { get; set; }
    }
}

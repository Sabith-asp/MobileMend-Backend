using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateTechnicianRequestDTO
    {
        public Guid TechnicianRequestId { get; set; }
        public   bool Status { get; set; }
        public string? AdminRemarks { get; set; } = null;
    }
}

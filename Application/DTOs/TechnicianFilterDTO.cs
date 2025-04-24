using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TechnicianFilterDTO
    {
        public Guid? technicianId { get; set; }
        public string? Search { get; set; }
    }
}

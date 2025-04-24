using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TechnicianDTO
    {
        public Guid TechnicianID { get; set; }
        public string TechnicianName { get; set; }

        public int Experience { get; set; } 
        public string Specialization { get; set; }
        public int CompletedJobs { get; set; }
        public int Rating { get; set; }
    }
}


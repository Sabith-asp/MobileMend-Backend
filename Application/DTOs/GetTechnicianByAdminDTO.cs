using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetTechnicianByAdminDTO
    {
        public Guid TechnicianId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Place { get; set; }
        public string Specialization { get; set; }
        public int Experience { get; set; }
        public bool IsBlocked { get; set; }
        public double Rating { get; set; }
        public int CompletedJobs { get; set; }
        public int PendingJobs { get; set; }

    }
}

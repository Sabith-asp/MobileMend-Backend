using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TechnicianAssignmentResult
    {
        public Guid TechnicianID { get; set; }
        public int PendingJobs { get; set; }
        public double Distance_Km { get; set; }
    }
}

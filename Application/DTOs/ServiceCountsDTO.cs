using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TechnicianServiceCountsDTO
    {
        public int Assigned { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TodayBookingStatusCountDTO
    {
        public int Accepted { get; set; }
        public int Rejected { get; set; }
        public int Completed { get; set; }
        public int InProgress { get; set; }
    }
}

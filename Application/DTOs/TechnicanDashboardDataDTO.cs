using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TechnicianDashboardDataDTO
    {
        public double TotalRevenue { get; set; }

        public TechnicianServiceCountsDTO TechnicianServiceCounts { get; set; }

        public IEnumerable<TechnicianRevenueChartDataDTO> TechnicianRevenueChartData { get; set; }
    }
}

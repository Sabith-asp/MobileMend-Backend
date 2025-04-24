using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AdminDashboardDataDTO
    {
        public double TotalRevenue { get; set; }
        public double TotalProfit { get; set; }
        public int ActiveTechnicians { get; set; }
        public int TotalCompletedBookings { get; set; }
        public IEnumerable<RevenueChartDataDTO> RevenueChartData { get; set; }
        public IEnumerable<PopularChartDataDTO> PopularChartData { get; set; }

        public TodayBookingStatusCountDTO TodayBookingCounts { get; set; }

    }
}

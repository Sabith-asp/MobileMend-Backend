using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces.Repositories
{
    public interface IAdminRepository
    {
        Task<AdminDashboardDataDTO> GetDashboardData();
        Task<List<RevenueChartDataDTO>> GetRevenueChartData();
        Task<List<PopularChartDataDTO>> GetPopularChartData();
        Task<TodayBookingStatusCountDTO> TodayBookingStatusCounts();
    }
}

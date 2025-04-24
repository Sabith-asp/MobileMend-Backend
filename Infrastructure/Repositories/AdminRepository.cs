using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class AdminRepository:IAdminRepository
    {
        private readonly DapperContext context;
        public AdminRepository(DapperContext _context)
        {
            context = _context;
        }
        public async Task<AdminDashboardDataDTO> GetDashboardData()
        {
            var totalRevenueQuery = "SELECT SUM(totalRevenue) AS TotalRevenue FROM (SELECT (BookingCharge + TravelAllowance + ServiceCharge) AS totalRevenue FROM BookingPricing) AS RevenueTable";
            var totalProfitQuery = "SELECT SUM(totalRevenue) AS TotalProfit FROM (SELECT (BookingCharge + TravelAllowance + (ServiceCharge * 0.4)) AS totalRevenue FROM BookingPricing) AS RevenueTable";
            var activeTechniciansQuery = "SELECT COUNT(*) FROM Technicians WHERE IsAvailable = 'Online'";
            var completedBookingCountQuery = "SELECT COUNT(*) FROM Bookings WHERE bookingStatus = 'Completed'";

            using var connection = context.CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                var totalRevenue = await connection.ExecuteScalarAsync<double>(totalRevenueQuery, transaction: transaction);
                var totalProfit = await connection.ExecuteScalarAsync<double>(totalProfitQuery, transaction: transaction);
                var activeTechnicians = await connection.ExecuteScalarAsync<int>(activeTechniciansQuery, transaction: transaction);
                var completedBookingCount = await connection.ExecuteScalarAsync<int>(completedBookingCountQuery, transaction: transaction);

                var dashboardData = new AdminDashboardDataDTO
                {
                    TotalRevenue = totalRevenue,
                    TotalProfit = totalProfit,
                    ActiveTechnicians = activeTechnicians,
                    TotalCompletedBookings = completedBookingCount
                };

                transaction.Commit();

                return dashboardData;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error fetching dashboard data", ex);
            }
        }


        public async Task<List<RevenueChartDataDTO>> GetRevenueChartData()
        {
            var chartDataQuery = @"
        SELECT
            MONTH(CreatedAt) AS Month,
            YEAR(CreatedAt) AS Year,
            SUM(BookingCharge + TravelAllowance + ServiceCharge) AS Revenue,
            SUM(ServiceCharge * 0.6) AS Expense
        FROM BookingPricing
        GROUP BY YEAR(CreatedAt), MONTH(CreatedAt)
        ORDER BY YEAR(CreatedAt), MONTH(CreatedAt);
    ";

            using var connection = context.CreateConnection();  // Assuming you're using Dapper
            var result = await connection.QueryAsync(chartDataQuery);

            var chartData = result.Select(r => new RevenueChartDataDTO
            {
                Month = GetMonthName((int)r.Month),  // Helper method to convert month number to month name
                Revenue = r.Revenue,
                Expense = r.Expense
            }).ToList();

            return chartData;
        }

        // Helper method to get the month name from the month number
        private string GetMonthName(int monthNumber)
        {
            var months = new[]
            {
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    };
            return months[monthNumber - 1];
        }



        public async Task<List<PopularChartDataDTO>> GetPopularChartData()
        {
            var sql = @"
        SELECT s.ServiceName AS Type, COUNT(*) AS Count
        FROM Bookings b
        JOIN Services s ON b.ServiceId = s.ServiceId
        WHERE b.bookingStatus = 'Completed'
        GROUP BY s.ServiceName
        ORDER BY Count DESC;
    ";

            using var connection = context.CreateConnection();
            var result = await connection.QueryAsync<PopularChartDataDTO>(sql);
            return result.ToList();
        }

        public async Task<TodayBookingStatusCountDTO> TodayBookingStatusCounts() {

            var sql = @"SELECT 
    SUM(CASE WHEN bookingStatus = 'Accepted' AND DATE(createdAt) = CURDATE() THEN 1 ELSE 0 END) AS Accepted,
    SUM(CASE WHEN bookingStatus = 'Rejected' AND DATE(createdAt) = CURDATE() THEN 1 ELSE 0 END) AS Rejected,
    SUM(CASE WHEN bookingStatus = 'Completed' AND DATE(createdAt) = CURDATE() THEN 1 ELSE 0 END) AS Completed,
    SUM(CASE WHEN bookingStatus NOT IN ('Accepted', 'Rejected', 'Completed') AND DATE(createdAt) = CURDATE() THEN 1 ELSE 0 END) AS InProgress
FROM Bookings;

";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<TodayBookingStatusCountDTO>(sql);
            
        }



    }
}

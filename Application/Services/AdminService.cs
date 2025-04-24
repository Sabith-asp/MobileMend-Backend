using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;

namespace Application.Services
{
    public class AdminService:IAdminService
    {
        private readonly IAdminRepository adminRepository;
        public AdminService(IAdminRepository _adminRepository)
        {
            adminRepository = _adminRepository;
        }

        public async Task<ResponseDTO<AdminDashboardDataDTO>> GetDashboardData()
        {
            try {
                var result=await adminRepository.GetDashboardData();
                result.RevenueChartData = await adminRepository.GetRevenueChartData();
                result.PopularChartData=await adminRepository.GetPopularChartData();
                result.TodayBookingCounts = await adminRepository.TodayBookingStatusCounts();

                return new ResponseDTO<AdminDashboardDataDTO> { StatusCode = 200, Message = "dashboard data retrieved", Data = result };
            
            }catch (Exception ex) {
                return new ResponseDTO<AdminDashboardDataDTO> { StatusCode = 500, Message = ex.Message  };

            }
        }
    }
}

using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Repositories;
using MobileMend.Application.Interfaces.Services;
using MobileMend.Domain.Entities;

namespace MobileMend.Application.Services
{
    public class TechnicianService:ITechnicianService
    {
        private readonly IMapper mapper;
        private readonly ITechnicianRepository technicianRepository;
        private readonly INotificationService notificationService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IBookingRepository bookingRepository;
        public TechnicianService(ITechnicianRepository _technicianRepository, INotificationService _notificationService,ICloudinaryService _cloudinaryService,IMapper _mapper,IBookingRepository _bookingRepository) {
            technicianRepository= _technicianRepository;
            notificationService= _notificationService;
            cloudinaryService = _cloudinaryService;
            mapper = _mapper;
            bookingRepository = _bookingRepository;
        }
        public async Task<ResponseDTO<object>> TechnicianRequest(string userId, TechnicianRequestCreateDTO newrequest) {
            try
            {
                
                var alreadyRequested = await technicianRepository.CheckAlreadyRequested(userId);
                if (alreadyRequested != null) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Already requested" };
                }
                var url = await cloudinaryService.UploadDocumentAsync(newrequest.Resume, "documents");
                Console.WriteLine(url);
                var data=mapper.Map<TechncianRequestAddDTO>(newrequest);
                data.UserID = userId;
                data.Resume = url;
                var result=await technicianRepository.TechnicianRequest(data);
                if (result < 1) { 
                 return new ResponseDTO<object> { StatusCode = 400, Message = "Error in technician request" };
                }
                return new ResponseDTO<object> { StatusCode = 200, Message = "Technician request success" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };

            }
        }


        public async Task<ResponseDTO<object>> UpdateRequestStatus(Guid technicianRequestId,bool status, string? adminRemarks) {
            try
            {
                var approved = status ? "Approved" : "Rejected";
                var result = await technicianRepository.UpdateRequestStatus(technicianRequestId,approved,adminRemarks);
                if (approved == "Approved")
                {
                    await technicianRepository.AddTechnician(technicianRequestId);
                }   
                if (result < 1) { 
                 return new ResponseDTO<object> { StatusCode = 400, Message = "Error in updating technician" };
                }
                return new ResponseDTO<object> { StatusCode = 200, Message = "Update request success" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };

            }
        }

        public async Task<ResponseDTO<IEnumerable<object>>> GetRequests(TechnicianRequestStatuses? status, string? search)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(search))
                {
                    search = null;
                }

                var result = await technicianRepository.GetRequests(status,search);
                if (result == null || !result.Any()) {
                    return new ResponseDTO<IEnumerable<object>>{ StatusCode=404,Message=$"requests not found"};
                }
              
                return new ResponseDTO<IEnumerable<object>> { StatusCode = 200, Message = $"requests retrieved",Data=result };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<object>> { StatusCode = 500, Error = ex.Message };

            }
        }


        public async Task<ResponseDTO<object>> UpdateServiceRequest(UpdateServiceRequestDTO statusdata)
        {
            try
            {
                string technicianDecision = statusdata.Status ? "Accepted" : "Rejected";

                var result=await technicianRepository.UpdateServiceRequest(statusdata.TechnicianId, statusdata, technicianDecision);

                var userid = await technicianRepository.GetUserIdByBookingID(statusdata.BookingID);
                if (result<1) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in updating technician desicion" };

                }
                if (!statusdata.Status)
                {
                    var newTechnician = await technicianRepository.ReassignTechnician(statusdata.BookingID);
                    var bookingDetail = await bookingRepository.GetBookingByID(statusdata.BookingID);
                    var bookingDetailForNotification = mapper.Map<NofityBookingToTechncianDTO>(bookingDetail);
                    await notificationService.NotifyTechnician(newTechnician.ToString(), bookingDetailForNotification);
                    return new ResponseDTO<object> { StatusCode = 200, Message = "Service rejected by technician and reassign to other technician" };
                }
                
                //await notificationService.NotifyTechnician(userid.ToString(), "📲 your service accepted by technician");


                return new ResponseDTO<object> { StatusCode = 200, Message = "Technician accepted serivce request" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };

            }
        }


        public async Task<ResponseDTO<object>> UpdateServiceStatus(Guid technicianId, ServiceStatus status, Guid bookingId)
        {
            try
            {

                var result=await technicianRepository.UpdateServiceStatus(technicianId,status, bookingId);
                var bookingDetail = await bookingRepository.GetBookingByID(bookingId);
                    if (result < 1)
                {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in updating service status" };

                }
                if (status.ToString() == "Completed") {
                    var userid=await technicianRepository.GetUserIdByBookingID(bookingId);
                }
                if (status.ToString() == "Completed" && bookingDetail.SparesTotal == 0) {
                    await bookingRepository.UpdatePayment(bookingId);
                }
               

                return new ResponseDTO<object> { StatusCode = 200, Message = "Service status updated" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };

            }
        }


        public async Task<ResponseDTO<object>> UpdateAvailability(string userid, UpdateAvailablityDTO status) {
            try {
                var technicianId = await technicianRepository.GetTechnicianIdByUserId(userid);
                var result = await technicianRepository.UpdateAvailability(technicianId, status);

                if (result < 1)
                {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in updating availability" };

                }
                return new ResponseDTO<object> { StatusCode = 200, Message = "Availability updated" };
            } catch (Exception ex) {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };

            }
        }

        public async Task<ResponseDTO<IEnumerable<TechnicianDTO>>> GetBestTechnician(Guid customerAddressId, Guid deviceId) {
            try
            {
                var result = await technicianRepository.GetBestTechnician(customerAddressId, deviceId);
                
                return new ResponseDTO<IEnumerable<TechnicianDTO>> { StatusCode = 200, Message = "Nearest technicians retrieved",Data=result };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<TechnicianDTO>> { StatusCode = 500, Error = ex.Message };

            }
        }

        public async Task<ResponseDTO<IEnumerable<GetTechnicianByAdminDTO>>> GetTechnicians(TechnicianFilterDTO filter)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(filter.Search))
                {
                    filter.Search = null;
                }
                var result = await technicianRepository.GetTechnicians(filter);

                return new ResponseDTO<IEnumerable<GetTechnicianByAdminDTO>> { StatusCode = 200, Message = "technicians retrieved", Data = result };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<GetTechnicianByAdminDTO>> { StatusCode = 500, Error = ex.Message };

            }
        }

        public async Task<ResponseDTO<object>> FindTechnician(Guid addressId, Guid deviceId) {
            try
            {
                var result = await technicianRepository.FindTechnician(addressId, deviceId);
                if (result == null) {
                    return new ResponseDTO<object> { StatusCode = 404, Message = "No technicians near to you"};
                }

                return new ResponseDTO<object> { StatusCode = 200, Message = "Nearest technicians retrieved", Data = result };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };

            }
        }


        public async Task<ResponseDTO<object>> UpdateCurrentLocation(UpdateCurrentLocationDTO currentLocation)
        {
            try
            {
                var result = await technicianRepository.UpdateCurrentLocation(currentLocation);
               

                if (result < 1)
                {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in updating current location" };

                }
                return new ResponseDTO<object> { StatusCode = 200, Message = "Location updated" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };

            }
        }


        public async Task<ResponseDTO<string>> ToggleTechnicianStatus(Guid technicianId)
        {
            try
            {
                var result = await technicianRepository.ToggleTechnicianStatus(technicianId);

                if (result<1)
                    return new ResponseDTO<string> { StatusCode = 404, Message = "Technician not found or update failed" };

                return new ResponseDTO<string> { StatusCode = 200, Message = "Technician status updated successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string> { StatusCode = 500, Error = ex.Message };
            }
        }

        public async Task<ResponseDTO<string>> RemoveTechnician(Guid technicianId)
        {
            try
            {
                var result = await technicianRepository.RemoveTechnician(technicianId);

                if (result<1)
                    return new ResponseDTO<string> { StatusCode = 404, Message = "Technician not found or deletion failed" };

                return new ResponseDTO<string> { StatusCode = 200, Message = "Technician removed successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string> { StatusCode = 500, Error = ex.Message };
            }
        }


        public async Task<ResponseDTO<TechnicianDashboardDataDTO>> GetTechnicianDashboardData(string userId)
        {
            try
            {
                var technicianId=await technicianRepository.GetTechnicianIdByUserId(userId);

                var result = await technicianRepository.GetTechnicianDashboardData(technicianId);
                var technicianChartData = await technicianRepository.GetMonthlyRevenueAndBookings(technicianId);
                result.TechnicianRevenueChartData = technicianChartData;

                return new ResponseDTO<TechnicianDashboardDataDTO> { StatusCode = 200, Message = "technician dashboard data retrieved", Data = result };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<TechnicianDashboardDataDTO> { StatusCode = 500, Error = ex.Message };

            }
        }


        public async Task<ResponseDTO<object>> NotifySparesPayment(Guid bookingId)
        {
            try
            {
                var bookingDetails=await bookingRepository.GetBookingByID(bookingId);
                var pay = new NotifySparesPaymentDTO { SparesTotal=bookingDetails.SparesTotal,BookingId= bookingId,Spares=bookingDetails.Spares };
                await notificationService.NotifyCustomer(bookingDetails.CustomerID.ToString(), pay);

                return new ResponseDTO<object> { StatusCode = 200, Message = "notified customer for spare payment"};
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };

            }
        }

    }
}

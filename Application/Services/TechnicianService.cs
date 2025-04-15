using Application.DTOs;
using Application.Interfaces.Services;
using Application.Services;
using AutoMapper;
using Domain.Enums;
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
        public TechnicianService(ITechnicianRepository _technicianRepository, INotificationService _notificationService,ICloudinaryService _cloudinaryService,IMapper _mapper) {
            technicianRepository= _technicianRepository;
            notificationService= _notificationService;
            cloudinaryService = _cloudinaryService;
            mapper = _mapper;
        }
        public async Task<ResponseDTO<object>> TechnicianRequest(TechnicianRequestCreateDTO newrequest) {
            try
            {
                
                var alreadyRequested = await technicianRepository.CheckAlreadyRequested(newrequest.UserID);
                if (alreadyRequested != null) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Already requested" };
                }
                var url = await cloudinaryService.UploadDocumentAsync(newrequest.Resume, "documents");
                Console.WriteLine(url);
                var data=mapper.Map<TechncianRequestAddDTO>(newrequest);
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


        public async Task<ResponseDTO<object>> UpdateRequestStatus(Guid technicianRequestId,bool status, string adminRemarks) {
            try
            {
                var approved = status ? "Approved" : "Rejected";
                var result = await technicianRepository.UpdateRequestStatus(technicianRequestId,approved,adminRemarks);
                if (approved == "Approved") {
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

        public async Task<ResponseDTO<IEnumerable<object>>> GetRequestsByStatus(TechnicianRequestStatuses status)
        {
            try
            {
                var result = await technicianRepository.GetRequestsByStatus(status);
                if (result == null || !result.Any()) {
                    return new ResponseDTO<IEnumerable<object>>{ StatusCode=404,Message=$"requests with {status} not found"};
                }
              
                return new ResponseDTO<IEnumerable<object>> { StatusCode = 200, Message = $"{status} request retrieved",Data=result };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<object>> { StatusCode = 500, Error = ex.Message };

            }
        }


        public async Task<ResponseDTO<object>> UpdateServiceRequest(Guid technicianID,UpdateServiceRequestDTO statusdata)
        {
            try
            {
                string technicianDecision = statusdata.Status ? "Accepted" : "Rejected";

                var result=await technicianRepository.UpdateServiceRequest(technicianID, statusdata, technicianDecision);

                var userid = await technicianRepository.GetUserIdByBookingID(technicianID);
                if (result<1) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in updating technician desicion" };

                }
                if (!statusdata.Status)
                {
                    await notificationService.NotifyTechnician(userid.ToString(), "📲 your service rejected by technician");
                    return new ResponseDTO<object> { StatusCode = 200, Message = "Service rejected by technician" };
                }
                
                await notificationService.NotifyTechnician(userid.ToString(), "📲 your service accepted by technician");


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

                if (result < 1)
                {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in updating service status" };

                }
                if (status.ToString() == "Completed") {
                    var userid=await technicianRepository.GetUserIdByBookingID(bookingId);
                    await notificationService.NotifyCustomer(userid.ToString(), "📲 your service completed. pay now");
                }
               

                return new ResponseDTO<object> { StatusCode = 200, Message = "Service status updated" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };

            }
        }


        public async Task<ResponseDTO<object>> UpdateAvailability(Guid technicianID, TechnicianAvailabilityStatus status) {
            var result = await technicianRepository.UpdateAvailability(technicianID,status);

            if (result < 1)
            {
                return new ResponseDTO<object> { StatusCode = 400, Message = "Error in updating service status" };

            }
            return new ResponseDTO<object> { StatusCode = 200, Message = "Service status updated" };
        }

    }
}

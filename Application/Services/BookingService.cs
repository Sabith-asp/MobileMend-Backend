using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MobileMend.Application.DTOs;
using MobileMend.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace Application.Services
{
    public class BookingService: IBookingService
    {
        private readonly IBookingRepository bookingRepository;
        private readonly INotificationService notificationService;
        private readonly IMapper mapper;
        public BookingService(IBookingRepository _bookingRepository,INotificationService _notificationService,IMapper _mapper ) {
            bookingRepository = _bookingRepository;
            notificationService = _notificationService;
            mapper= _mapper;
        }
        public int bookingcharge = 199;
        public int travelAllowancePerKm=20;
        public async Task<ResponseDTO<object>> BookService(string userId,BookingCreateDTO newbooking)
        {
            try {
                var distance = await bookingRepository.GetBookingDistance(newbooking.TechnicianID, newbooking.AddressID);
                
                double travelAllowance = distance * travelAllowancePerKm;
                var bookingresult=await bookingRepository.BookService(userId,bookingcharge, newbooking, travelAllowance, distance,newbooking.TechnicianID);
                if (bookingresult.RowsAffected< 1) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in booking service" };

                }
                var bookingDetails = await bookingRepository.GetBookingByID(bookingresult.BookingId);
                var bookingDetailToNotifyTechnciian=mapper.Map<NofityBookingToTechncianDTO>(bookingDetails);
                await notificationService.NotifyTechnician(newbooking.TechnicianID.ToString(), bookingDetailToNotifyTechnciian);
                return new ResponseDTO<object> { StatusCode = 200, Message = "Service booked" };


            } catch (Exception ex) {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };

            }
        }


        public async Task<ResponseDTO<object>> GetBooking(string? userId,string? Role, Guid? bookingId, ServiceStatus? status, Guid? technicianId, string? searchString) {

            try {
                if (bookingId.HasValue)
                {
                    var booking = await bookingRepository.GetBookingByID(bookingId);
                    return new ResponseDTO<object> { StatusCode = 200, Message = "booking retrieved", Data = booking };
                }
               

                else if (technicianId != null && technicianId != Guid.Empty && status != null )
                {
                    Console.WriteLine(technicianId);
                    var bookingsByTechnicianAndStatus = await bookingRepository.GetBookingsByTechnicianIdAndStatus(technicianId, status);
                    return new ResponseDTO<object> { StatusCode = 200, Message = "Bookings by technician and status retrieved", Data = bookingsByTechnicianAndStatus.OrderByDescending(data => data.CreatedAt) };
                }

                else if (status == ServiceStatus.InProgress && Role=="Admin")
                {
                    var bookingsByStatus = await bookingRepository.GetBookingsInProgress(technicianId, status, searchString);
                    return new ResponseDTO<object> { StatusCode = 200, Message = "booking In Progress retrieved", Data = bookingsByStatus.OrderByDescending(data => data.CreatedAt) };
                }

                else if (status != null)
                {

                    var bookingsByStatus = await bookingRepository.GetBookingsByStatus(status, searchString);
                    return new ResponseDTO<object> { StatusCode = 200, Message = "bookings by status retrieved", Data = bookingsByStatus.OrderByDescending(data => data.CreatedAt) };
                }
                else if (Role == "Admin")
                {
                    var result = await bookingRepository.GetAllBookings(searchString);
                    return new ResponseDTO<object> { StatusCode = 200, Message = "all bookings retrieved", Data = result.OrderByDescending(data => data.CreatedAt) };
                }

                else if (userId != null)
                {
                    var userBookings = await bookingRepository.GetBookingsByUserID(userId);
                    return new ResponseDTO<object> { StatusCode = 200, Message = "user bookings retrieved", Data = userBookings.OrderByDescending(data => data.CreatedAt) };
                }
              
                else {
                    return new ResponseDTO<object> { StatusCode = 404 };

                }




            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }
        }

        public async Task<ResponseDTO<object>> GetBookingEstimate(Guid technicianId, Guid addressId,Guid serviceId) {
            try
            {
  
                var distanceInKm = await bookingRepository.GetBookingDistance(technicianId, addressId);
                var travelAllowance = distanceInKm * travelAllowancePerKm;
                var serviceCharge = await bookingRepository.GetServiceCharge(serviceId);
                var totalCost=bookingcharge+travelAllowance+ serviceCharge;
                return new ResponseDTO<object> { StatusCode = 200, Message = "booking estimate retrieved", Data = new {serviceCharge= serviceCharge, bookingCharge= bookingcharge, travelAllowance= Math.Round(travelAllowance, 1),totalDistance= Math.Round(distanceInKm, 1)  ,totalCost= Math.Round(totalCost, 1)   } };

            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }
        }

        public async Task<ResponseDTO<object>> UpdatePayment(Guid bookingId)
        {
            try
            {
                var result=await bookingRepository.UpdatePayment(bookingId);
                if (result < 1) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Payment is not updated" };

                }
                return new ResponseDTO<object> { StatusCode = 200, Message = "Payment is updated" };

            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }
        }

    }
}

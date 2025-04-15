using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using MobileMend.Application.DTOs;

namespace Application.Services
{
    public class BookingService: IBookingService
    {
        private readonly IBookingRepository bookingRepository;
        private readonly INotificationService notificationService;
        public BookingService(IBookingRepository _bookingRepository,INotificationService _notificationService ) {
            bookingRepository = _bookingRepository;
            notificationService = _notificationService;
        }
        public async Task<ResponseDTO<object>> BookService(Guid userId,BookingCreateDTO newbooking)
        {
            try {
                var technician = new TechnicianAssignmentResult();
                if (newbooking.TechnicianID != null)
                {
                    technician = await bookingRepository.GetSelectedTechnicianInfo(newbooking.TechnicianID, newbooking.AddressID);
                }
                else { technician = await bookingRepository.FindTechnician(newbooking.AddressID, newbooking.DeviceID); }

                 
                if (technician == null) {
                    
                    return new ResponseDTO<object> { StatusCode = 404, Message = "No technicians near to you" };

                }
                Console.WriteLine(technician.TechnicianID);
                double bookingcharge = 199;
                double travelAllowance = technician.Distance_Km * 10;
                await notificationService.NotifyCustomer(userId.ToString(),$"Pay the advance amount {bookingcharge+travelAllowance} now");
                var result=await bookingRepository.BookService(bookingcharge, newbooking, travelAllowance, technician.Distance_Km,technician.TechnicianID);
                if (result<1) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in booking service" };

                }

                await notificationService.NotifyTechnician(technician.TechnicianID.ToString(), "📲 New booking assigned to you!");
                return new ResponseDTO<object> { StatusCode = 200, Message = "Service booked" };


            } catch (Exception ex) {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };

            }
        }


        public async Task<ResponseDTO<object>> GetBooking(string? userId, Guid? bookingId, string? status) {

            try {
                if (bookingId.HasValue) { 
                var booking=await bookingRepository.GetBookingByID(bookingId);
                    return new ResponseDTO<object> { StatusCode = 200, Message = "booking retrieved", Data = booking };
                }
                if (userId!=null)
                {
                    var userBookings = await bookingRepository.GetBookingsByUserID(userId);
                    return new ResponseDTO<object> { StatusCode = 200, Message = "user bookings retrieved", Data = userBookings };
                }
                if (status!=null) {
                    var bookingsByStatus = await bookingRepository.GetBookingsByStatus(status);
                    return new ResponseDTO<object> { StatusCode = 200, Message = "bookings by status retrieved", Data = bookingsByStatus };
                }


                var result= await bookingRepository.GetAllBookings();
                return new ResponseDTO<object> { StatusCode = 200, Message = "bookings retrieved",Data=result };

            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }
        }

       
    }
}

using Application.DTOs;
using Dapper;
using Domain.Enums;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Repositories;
using MobileMend.Domain.Entities;
using MobileMend.Infrastructure.Data;

namespace MobileMend.Infrastructure.Repositories
{
    public class TechnicianRepository : ITechnicianRepository
    {
        private readonly DapperContext context;
        public TechnicianRepository(DapperContext _context) { context = _context; }
        public async Task<int> TechnicianRequest(TechncianRequestAddDTO newrequest) {
            var sql = "insert into technicianRequests (TechnicianRequestID,UserID,Experience,ResumeUrl,Specialization,Bio,Longitude,Latitude) values (UUID(),@UserID,@Experience,@ResumeUrl,@Specialization,@Bio,@Longitude,@Latitude)";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { UserID = newrequest.UserID, Experience = newrequest.Experience, ResumeUrl = newrequest.Resume, Specialization = newrequest.Specialization, Bio = newrequest.Bio, Longitude = newrequest.Longitude, Latitude = newrequest.Latitude });
            return rowsaffected;
        }

        public async Task<TechnicianRequest> CheckAlreadyRequested(Guid userid) {
            var sql = "select * from TechnicianRequests where userid=@userid";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<TechnicianRequest>(sql, new { userid = userid });
        }

        public async Task<int> UpdateRequestStatus(Guid technicianRequestId, string status, string adminRemark)
        {
            var sql = "update technicianrequests set status=@Status,adminRemark=@AdminRemark where technicianRequestID=@TechnicianRequestID";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { TechnicianRequestID = technicianRequestId, Status = status, AdminRemark = adminRemark });
            Console.WriteLine(rowsaffected);
            return rowsaffected;
        }


        public async Task<IEnumerable<TechnicianRequest>> GetRequestsByStatus(TechnicianRequestStatuses status)
        {
            var sql = "select * from TechnicianRequests where status=@Status";
            using var connection = context.CreateConnection();
            return await connection.QueryAsync<TechnicianRequest>(sql, new { Status = status.ToString() });
        }


        public async Task<int> AddTechnician(Guid technicianRequestId)
        {
            //var technicianDetailQuery = "select * from technicianrequests where technicianRequestId=@TechnicianRequestId ";
            var sql = "insert into Technicians (TechnicianID,UserID,Specialization,Experience,Bio) select UUID(),UserID,Specialization,Experience,Bio from TechnicianRequests where TechnicianRequestID=@TechnicianRequestID";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { TechnicianRequestID = technicianRequestId });
            Console.WriteLine(rowsaffected);
            return rowsaffected;
        }

        public async Task<int> UpdateServiceRequest(Guid TechnicianID, UpdateServiceRequestDTO statusdata, string technicianDecision)
        {
            var updateDesicionQuery = "update bookings set bookingstatus=@BookingStatus where bookingid=@BookingID";
            var addToActionTableQuery = "insert into ServiceRequestActions (Id,BookingID,TechnicianID,Reason,Decision) values (UUID(),@BookingID,@TechnicianID,@Reason,@Desicion)";
            var updatePendingJobsQuery = "update technicians set pendigjobs=pendingjobs+1 where technicianid=@TechnicianID";

            using var connection = context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try {
                if (statusdata.Status) {
                    await connection.ExecuteAsync(updatePendingJobsQuery, new { TechnicianID = TechnicianID });
                }

                await connection.ExecuteAsync(updateDesicionQuery, new { BookingStatus = technicianDecision, BookingID = statusdata.BookingID });



                var rowsaffected = await connection.ExecuteAsync(addToActionTableQuery, new { BookingID = statusdata.BookingID, TechnicianID = TechnicianID, Reason = statusdata.Status ? null : statusdata.RejectionReason, Desicion = statusdata.Status });
                Console.WriteLine(rowsaffected);
                transaction.Commit();
                return rowsaffected;

            } catch (Exception ex) {

                transaction.Rollback();
                throw;
            }


        }


        public async Task<Guid> GetUserIdByBookingID(Guid bookingid)
        {
            var sql = "select CustomerID from Bookings where bookingid=@BookingID";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Guid>(sql, new { BookingID = bookingid });
        }


        public async Task<int> UpdateServiceStatus(Guid technicianId, ServiceStatus status, Guid bookingId)
        {
            Console.WriteLine(status);
            var sql = "update bookings set BookingStatus=@Status,UpdatedAt=NOW(),UpdatedBy=@TechnicianID where BookingID=@BookingID";
            var pendingJobDecrementQuery = "update technicians set pendingjobs=pendingjobs-1 where technicianid=@TechnicianID";
            using var connection = context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try {
                if (status.ToString() == "Completed") {
                    await connection.ExecuteAsync(pendingJobDecrementQuery, new { TechnicianID = technicianId });
                }


                var rowsaffected = await connection.ExecuteAsync(sql, new { Status = status.ToString(), TechnicianID = technicianId, BookingID = bookingId });
                transaction.Commit();
                return rowsaffected;
            }
            catch (Exception ex) {
                transaction.Rollback();
                throw;
            }

        }


        public async Task<int> UpdateAvailability(Guid technicianID, TechnicianAvailabilityStatus status) {
            Console.WriteLine(status);
            var sql = "update technicians set IsAvailable=@Status where technicianid=@TechnicianID";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new {Status=status.ToString(),TechnicianID= technicianID });
            return rowsaffected;
        }


    }
}

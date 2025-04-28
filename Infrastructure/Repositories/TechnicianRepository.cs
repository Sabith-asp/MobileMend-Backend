using System.Data.Common;
using System.Reflection.Metadata;
using System.Transactions;
using System.Xml.Linq;
using Application.DTOs;
using Dapper;
using Domain.Entities;
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
        public async Task<int> TechnicianRequest(TechncianRequestAddDTO newrequest)
        {
            var sql = "insert into TechnicianRequests (TechnicianRequestID,UserID,Experience,Specialization,Bio,Longitude,Latitude,Place,DocumentId,ResumeUrl) values (UUID(),@UserID,@Experience,@Specialization,@Bio,@Longitude,@Latitude,@Place,@DocumentId,'Url')";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { UserID = newrequest.UserID, Experience = newrequest.Experience, Specialization = newrequest.Specialization, Bio = newrequest.Bio, Longitude = newrequest.Longitude, Latitude = newrequest.Latitude, Place = newrequest.Place, DocumentId=newrequest.DocumentId });
            return rowsaffected;
        }

        public async Task<TechnicianRequest> CheckAlreadyRequested(string userid)
        {
            var sql = "select * from TechnicianRequests where userid=@userid";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<TechnicianRequest>(sql, new { userid = userid });
        }

        public async Task<int> UpdateRequestStatus(Guid technicianRequestId, string status, string? adminRemark)
        {
            var sql = "update TechnicianRequests set status=@Status,adminRemark=@AdminRemark where technicianRequestID=@TechnicianRequestID";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { TechnicianRequestID = technicianRequestId, Status = status, AdminRemark = adminRemark });
            Console.WriteLine(rowsaffected);
            return rowsaffected;
        }


        public async Task<IEnumerable<TechnicianRequest>> GetRequests(TechnicianRequestStatuses? status, string? search)
        {
            var sql = @"SELECT 
        t.TechnicianRequestID,
        t.UserID,
        t.Experience,
        d.Data AS DocumentData,
        u.Email AS Email,
        u.Name,
        u.Phone AS Phone,
        t.Specialization,
        t.Bio,
        t.Status,
        t.Place,
        t.AdminRemark,
        t.Longitude,
        t.Latitude,
        t.RequestDate
    FROM TechnicianRequests t
    LEFT JOIN Documents d ON d.DocumentId = t.DocumentId
    JOIN Users u ON u.UserID = t.UserID
    WHERE (@Status IS NULL OR t.Status = @Status)
      AND (@Search IS NULL OR 
           u.Name LIKE CONCAT('%', @Search, '%') OR 
           u.Email LIKE CONCAT('%', @Search, '%') OR 
           u.Phone LIKE CONCAT('%', @Search, '%') OR 
           t.Specialization LIKE CONCAT('%', @Search, '%') OR
           t.Place LIKE CONCAT('%', @Search, '%'))
    ORDER BY t.RequestDate DESC;";

            using var connection = context.CreateConnection();
            return await connection.QueryAsync<TechnicianRequest>(
                sql,
                new { Status = status.HasValue ? status.ToString() : null, Search = search }
            );
        }



        public async Task<int> AddTechnician(Guid technicianRequestId)
        {
            //var technicianDetailQuery = "select * from technicianrequests where technicianRequestId=@TechnicianRequestId ";
            var sql = "insert into Technicians (TechnicianID,UserID,Specialization,Experience,Bio,Place) select UUID(),UserID,Specialization,Experience,Bio,Place from TechnicianRequests where TechnicianRequestID=@TechnicianRequestID";
            var updateRoleQuery = "update Users set Role='Technician' where UserID=(select UserID from TechnicianRequests where TechnicianRequestID=@TechnicianRequestID)";
            using var connection = context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                await connection.ExecuteAsync(sql, new { TechnicianRequestID = technicianRequestId }, transaction);
                var rowsaffected = await connection.ExecuteAsync(updateRoleQuery, new { TechnicianRequestID = technicianRequestId }, transaction);
                transaction.Commit();
                return rowsaffected;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
            //var rowsaffected = await connection.ExecuteAsync(sql, new { TechnicianRequestID = technicianRequestId });

        }

        public async Task<int> UpdateRoleToTechnician(Guid userid)
        {
            var sql = "update Users set Role='Technician' where UserID=@UserId";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { UserId = userid });
            Console.WriteLine(rowsaffected);
            return rowsaffected;
        }

        public async Task<int> UpdateServiceRequest(Guid TechnicianID, UpdateServiceRequestDTO statusdata, string technicianDecision)
        {
            var updateDesicionQuery = "update Bookings set bookingstatus=@BookingStatus where bookingid=@BookingID";
            var addToActionTableQuery = "insert into ServiceRequestActions (Id,BookingID,TechnicianID,Reason,Decision) values (UUID(),@BookingID,@TechnicianID,@Reason,@Desicion)";
            var updatePendingJobsQuery = "update Technicians set PendingJobs=PendingJobs+1 where technicianid=@TechnicianID";

            using var connection = context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                if (statusdata.Status)
                {
                    await connection.ExecuteAsync(updatePendingJobsQuery, new { TechnicianID = TechnicianID }, transaction);
                }

                await connection.ExecuteAsync(updateDesicionQuery, new { BookingStatus = technicianDecision, BookingID = statusdata.BookingID }, transaction);



                var rowsaffected = await connection.ExecuteAsync(addToActionTableQuery, new { BookingID = statusdata.BookingID, TechnicianID = TechnicianID, Reason = statusdata.Status ? null : statusdata.RejectionReason, Desicion = statusdata.Status }, transaction);
                Console.WriteLine(rowsaffected);
                transaction.Commit();
                return rowsaffected;

            }
            catch (Exception ex)
            {

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
            var sql = "update Bookings set BookingStatus=@Status,UpdatedAt=NOW(),UpdatedBy=@TechnicianID where BookingID=@BookingID";
            var jobCountUpdateQuery = "update Technicians set pendingjobs=pendingjobs-1,CompletedJobs=CompletedJobs+1 where technicianid=@TechnicianID";
            using var connection = context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                if (status.ToString() == "Completed")
                {
                    await connection.ExecuteAsync(jobCountUpdateQuery, new { TechnicianID = technicianId }, transaction);
                }


                var rowsaffected = await connection.ExecuteAsync(sql, new { Status = status.ToString(), TechnicianID = technicianId, BookingID = bookingId }, transaction);
                transaction.Commit();
                return rowsaffected;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }

        }


        public async Task<int> UpdateAvailability(Guid technicianID, UpdateAvailablityDTO status)
        {
            Console.WriteLine(status);
            var sql = "update Technicians set IsAvailable=@Status where technicianid=@TechnicianID";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { Status = status.TechnicianAvailabilityStatus.ToString(), TechnicianID = technicianID });
            return rowsaffected;
        }


        public async Task<IEnumerable<TechnicianDTO>> GetBestTechnician(Guid customerAddressId, Guid deviceId)
        {



            var sql = @"SELECT 
                                TechnicianID,
                                TechnicianName,
                                Experience,
                                Specialization,
                                CompletedJobs,
                                Rating,
                                Distance
                            FROM (
                                SELECT 
                                    t.TechnicianID,
                                    u.Name AS TechnicianName,
                                    t.Experience,
                                    t.Specialization,
                                    t.CompletedJobs,
                                    t.AverageRating AS Rating,
                                    (
                                        6371 * ACOS(
                                            COS(RADIANS(a.Latitude)) * 
                                            COS(RADIANS(t.CurrentLatitude)) * 
                                            COS(RADIANS(t.CurrentLongitude) - RADIANS(a.Longitude)) + 
                                            SIN(RADIANS(a.Latitude)) * 
                                            SIN(RADIANS(t.CurrentLatitude))
                                        )
                                    ) AS Distance
                                FROM Technicians t
                                JOIN Users u ON u.UserId = t.UserId
                                JOIN Addresses a ON a.AddressId = @CustomerAddressId
                                JOIN Devices d ON d.DeviceId = @DeviceId
                                WHERE 
                                    t.IsAvailable = 'Online' AND
                                    t.IsBlocked = FALSE AND
                                    t.IsDeleted = FALSE AND 
                                    t.PendingJobs < 1 AND 
                                    t.Specialization LIKE CONCAT('%', d.DeviceType, '%')
                            ) AS bestTechnician
                            WHERE Distance <= 15
                            ORDER BY Rating DESC, Distance ASC
                            LIMIT 5;";

            using var connection = context.CreateConnection();
            return await connection.QueryAsync<TechnicianDTO>(sql, new { CustomerAddressId = customerAddressId, DeviceId = deviceId });
        }


        public async Task<TechnicianAssignmentResult> FindTechnician(Guid addressID, Guid deviceID, IEnumerable<Guid>? alreadyAssigned = null)
        {
            var sql = @"
    SELECT TechnicianID, PendingJobs, Distance_Km FROM (
        SELECT 
            t.TechnicianID,
            t.PendingJobs,
            (
                6371 * ACOS(
                    COS(RADIANS(a.Latitude)) * 
                    COS(RADIANS(t.CurrentLatitude)) * 
                    COS(RADIANS(t.CurrentLongitude) - RADIANS(a.Longitude)) + 
                    SIN(RADIANS(a.Latitude)) * 
                    SIN(RADIANS(t.CurrentLatitude))
                )
            ) AS Distance_Km
        FROM 
            Technicians t
            JOIN Devices d ON d.DeviceID = @DeviceID
            JOIN Addresses a ON a.AddressID = @AddressID
        WHERE 
            t.IsAvailable = 'Online' AND 
            t.IsBlocked = FALSE AND
            t.IsDeleted = FALSE AND 
            t.PendingJobs < 4 AND 
            t.Specialization LIKE CONCAT('%', d.DeviceType, '%')";

            if (alreadyAssigned != null && alreadyAssigned.Any())
            {
                sql += " AND t.TechnicianID NOT IN @AlreadyAssigned";
            }

            sql += @"
    ) AS nearest
    WHERE nearest.distance_km <= 15
    ORDER BY distance_km ASC, PendingJobs ASC
    LIMIT 1";

            var parameters = new
            {
                DeviceID = deviceID,
                AddressID = addressID,
                AlreadyAssigned = alreadyAssigned?.ToArray()
            };

            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<TechnicianAssignmentResult>(sql, parameters);
        }



        public async Task<Guid> GetTechnicianIdByUserId(string Userid)
        {
            var sql = "select TechnicianID from Technicians where userid=@UserId";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Guid>(sql, new { UserId = Userid });
        }


        public async Task<int> UpdateCurrentLocation(UpdateCurrentLocationDTO currentLocation)
        {
            var sql = "update Technicians set CurrentLatitude=@Latitude ,CurrentLongitude=@Longitude,LastLocationUpdated=NOW() where technicianid=@TechnicianID";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { Latitude = currentLocation.Latitude, Longitude = currentLocation.Longitude, TechnicianID = currentLocation.TechnicianId });
            return rowsaffected;
        }


        public async Task<IEnumerable<GetTechnicianByAdminDTO>> GetTechnicians(TechnicianFilterDTO filter)
        {
            var sql = @"SELECT 
    t.TechnicianId,
    u.Name,
    u.Email,
    u.Phone,
    t.Specialization,
    t.Experience,
    t.AverageRating AS Rating,
    t.Place,
    t.CompletedJobs,
    t.PendingJobs,
    t.isBlocked
FROM Technicians t
JOIN Users u ON u.UserID = t.UserID
WHERE 
    (@TechnicianId IS NULL OR t.TechnicianId = @TechnicianId)
    AND (
        @Search IS NULL OR 
        u.Name LIKE CONCAT('%', @Search, '%') OR 
        u.Email LIKE CONCAT('%', @Search, '%') OR 
        t.Specialization LIKE CONCAT('%', @Search, '%')
    ) and t.isDeleted=FALSE
";
            using var connection = context.CreateConnection();
            return await connection.QueryAsync<GetTechnicianByAdminDTO>(sql, filter);
        }





        public async Task<int> ToggleTechnicianStatus(Guid technicianId)
        {
            var sql = @"
        UPDATE Technicians 
        SET IsBlocked = NOT IsBlocked
        WHERE TechnicianId = @TechnicianId;";

            using var connection = context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { TechnicianId = technicianId });
            return rowsAffected;
        }

        public async Task<int> RemoveTechnician(Guid technicianId)
        {
            var sql = @"
            update Technicians 
            set isDeleted = TRUE where TechnicianId=@TechnicianId;";

            using var connection = context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { TechnicianId = technicianId });
            return rowsAffected;
        }



        public async Task<TechnicianDashboardDataDTO> GetTechnicianDashboardData(Guid technicianId)
        {
            using var connection = context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                var totalRevenueQuery = @"
                        SELECT SUM(serviceCharge * 0.60) AS TotalRevenue
                        FROM BookingPricing
                        WHERE bookingId IN (SELECT bookingId FROM Bookings WHERE technicianid = @TechnicianId)"
                ;

                var totalRevenue = await connection.QueryFirstOrDefaultAsync<double?>(totalRevenueQuery, new { TechnicianId = technicianId }, transaction);
                if (totalRevenue == null)
                {
                    totalRevenue = 0;
                }
                var assignedQuery = @"
                        SELECT COUNT(*) 
                        FROM Bookings 
                        WHERE bookingStatus = 'Assigned' AND technicianid = @TechnicianId"
                ;

                var assigned = await connection.QueryFirstOrDefaultAsync<int>(assignedQuery, new { TechnicianId = technicianId }, transaction);

                var inProgressQuery = @"
                        SELECT COUNT(*) 
                        FROM Bookings 
                        WHERE bookingStatus NOT IN ('Assigned', 'Rejected', 'Completed','Reassigned') AND technicianid = @TechnicianId"
                ;

                var inProgress = await connection.QueryFirstOrDefaultAsync<int>(inProgressQuery, new { TechnicianId = technicianId }, transaction);

                var completedQuery = @"
                        SELECT COUNT(*) 
                        FROM Bookings 
                        WHERE bookingStatus = 'Completed' AND technicianid = @TechnicianId"
                ;

                var completed = await connection.QueryFirstOrDefaultAsync<int>(completedQuery, new { TechnicianId = technicianId }, transaction);

                transaction.Commit();

                return new TechnicianDashboardDataDTO
                {
                    TotalRevenue = totalRevenue,
                    TechnicianServiceCounts = new TechnicianServiceCountsDTO
                    {
                        Assigned = assigned,
                        InProgress = inProgress,
                        Completed = completed
                    }
                };
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }


        public async Task<IEnumerable<TechnicianRevenueChartDataDTO>> GetMonthlyRevenueAndBookings(Guid technicianId)
        {
            var sql = @"
            SELECT 
        MONTHNAME(b.createdat) AS Month, 
        COUNT(b.bookingid) AS Bookings, 
        COALESCE(SUM(bp.serviceCharge * 0.6), 0) AS Revenue
    FROM Bookings  b 
    JOIN BookingPricing bp ON b.bookingid = bp.bookingid
    WHERE b.technicianId=@TechnicianId
    GROUP BY MONTH(b.createdat), MONTHNAME(b.createdat)
    ORDER BY MONTH(b.createdat) "
            ;
            using var connection = context.CreateConnection();

            var chartData = await connection.QueryAsync<TechnicianRevenueChartDataDTO>(sql, new { TechnicianId = technicianId });

            return chartData;
        }

        public async Task<Guid> ReassignTechnician(Guid bookingId)
        {

            var alreadyAssignedTechniciansQuery = @"select TechnicianID from ServiceRequestActions WHERE BookingID=@BookingId";
            var bookingDetailQuery = @"select DeviceId,AddressId from Bookings where BookingId=@BookingId";
            var updateBookingQuery = @"update Bookings set BookingStatus='Reassigned',TechnicianId=@TechnicianId WHERE BookingId=@BookingID";
            using var connection = context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                IEnumerable<Guid>? alreadyAssigned = await connection.QueryAsync<Guid>(alreadyAssignedTechniciansQuery, new { BookingId = bookingId });
                var bookindDetailForReaasignment = await connection.QueryFirstOrDefaultAsync<BookingReassignmentDataDTO>(bookingDetailQuery, new { BookingId = bookingId });
                TechnicianAssignmentResult newTechnician = await FindTechnician(bookindDetailForReaasignment.AddressId, bookindDetailForReaasignment.DeviceId, alreadyAssigned);
                if (newTechnician == null)
                {
                    transaction.Rollback();
                    throw new InvalidOperationException("No available technician found for reassignment.");
                }
                await connection.ExecuteAsync(updateBookingQuery, new { TechnicianId = newTechnician.TechnicianID, BookingID = bookingId });
                transaction.Commit();
                return newTechnician.TechnicianID;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

        }


        public async Task<int> UploadDocument(Domain.Entities.Document documentData)
        {
            var sql = @"insert into Documents (DocumentId,Name,MimeType,Data) values (@DocumentId,@Name,@MimeType,@Data)";

            using var connection = context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { DocumentId = documentData.DocumentId, Name= documentData.Name, MimeType= documentData.MimeType, Data= documentData.Data });
            return rowsAffected;
        }




    }
}

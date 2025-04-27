using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Domain.Entities;
using MobileMend.Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DapperContext context;
        public BookingRepository(DapperContext _context)
        {
            context = _context;
        }
        public async Task<BookingResultDTO> BookService(string userId, double bookingCharge,BookingCreateDTO newbooking, double travelAllowance,double distance, Guid technicianid)
        {
            Guid bookingId = Guid.NewGuid();
            var serviceChargeQuery = "select Price from Services where serviceid=@ServiceID";
            var bookingQuery = @"
INSERT INTO Bookings (BookingID, Email, Issue, CustomerName, Phone, AddressID, CustomerID, DeviceID, ServiceID, TechnicianID, PaymentStatus)
SELECT 
    @BookingID,
    u.Email,
    @Issue,
    u.Name,
    u.Phone,
    @AddressID,
    u.UserID,
    @DeviceID,
    @ServiceID,
    @TechnicianID,
    'BookingPaid'
FROM Users u
WHERE u.UserID = @UserID";

            var bookingPriceAddingQuery = @"INSERT INTO BookingPricing (BookingPricingID, BookingID, BookingCharge, TravelAllowance, ServiceCharge,DistanceInKm) VALUES (UUID(),@BookingID,@BookingCharge,@TravelAllowance,@ServiceCharge,@DistanceInKm)";

            using var connection=context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try {
                var servicecharge = await connection.QueryFirstOrDefaultAsync<double>(serviceChargeQuery, new { ServiceID = newbooking.ServiceID }, transaction);
                Console.WriteLine(servicecharge);
                await connection.ExecuteAsync(bookingQuery, new { BookingID = bookingId, Issue = newbooking.Issue, AddressID = newbooking.AddressID, CustomerID= userId, DeviceID = newbooking.DeviceID, ServiceID = newbooking.ServiceID, TechnicianID = technicianid,UserID= userId }, transaction);

                var rowsaffected = await connection.ExecuteAsync(bookingPriceAddingQuery, new { BookingID = bookingId, BookingCharge = bookingCharge, TravelAllowance = travelAllowance, ServiceCharge = servicecharge, DistanceInKm = distance }, transaction);
                Console.WriteLine(rowsaffected);
                transaction.Commit();
                return new BookingResultDTO { BookingId= bookingId ,RowsAffected=rowsaffected};
            }
            catch (Exception ex) {
                transaction.Rollback();
                Console.WriteLine("Transaction failed: " + ex.Message);
                throw;
            }

        }

        public async Task<TechnicianAssignmentResult> FindTechnician(Guid addressID, Guid deviceID)
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
            t.IsDeleted = FALSE AND 
            t.PendingJobs < 4 AND 
            t.Specialization LIKE CONCAT('%', d.DeviceType, '%')
    ) AS nearest
    WHERE nearest.distance_km <= 15
    ORDER BY distance_km ASC, PendingJobs ASC
    LIMIT 1";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<TechnicianAssignmentResult>(sql, new { DeviceID = deviceID, AddressID = addressID });
        }


        public async Task<TechnicianAssignmentResult> GetSelectedTechnicianInfo(Guid? technicianid,Guid addressid) {
            var sql = @"
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
            JOIN Addresses a ON a.AddressID = @AddressID where t.TechnicianID = @TechnicianID";

            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<TechnicianAssignmentResult>(sql, new {  AddressID = addressid, TechnicianID = technicianid });



        }


        public async Task<IEnumerable<GetBookingDetailsDTO>> GetAllBookings(string? searchString) {

            var sql = @"
SELECT b.bookingid, b.customername,b.CustomerID, b.email, b.phone, b.issue, b.bookingstatus,
       b.paymentstatus, b.createdat, a.street, a.city, a.pincode,
       d.devicename, d.devicetype, s.servicename,
       u.name AS technicianName, u.phone AS technicianPhone,t.technicianId
FROM Bookings b
JOIN Addresses a ON b.addressid = a.addressid
JOIN Devices d ON d.deviceid = b.deviceid
JOIN Services s ON s.serviceid = b.serviceid
JOIN Technicians t ON t.technicianid = b.technicianid
JOIN Users u ON t.userid = u.userid
WHERE (@SearchString IS NULL OR 
       d.devicename LIKE CONCAT('%', @SearchString, '%') OR 
       b.customername LIKE CONCAT('%', @SearchString, '%') OR 
       s.servicename LIKE CONCAT('%', @SearchString, '%'))";

            if (string.IsNullOrWhiteSpace(searchString))
            {
                searchString = null;
            }

            using var connection = context.CreateConnection();
            var bookingresults= await connection.QueryAsync<GetBookingDetailsDTO>(sql, new { SearchString= searchString });
            foreach (var booking in bookingresults)
            {
                if (booking.BookingStatus == "Started" || booking.BookingStatus == "Completed")
                {
                    var spareQuery = @"SELECT ID, SpareName, Price, Qty, TotalCost FROM UsedSpares WHERE bookingid = @BookingID";
                    var spares = await connection.QueryAsync<GetSpareDTO>(spareQuery, new { BookingID = booking.BookingID });
                    booking.Spares = spares;
                    booking.SparesTotal = spares.Sum(x=>x.TotalCost);

                }
                var bookingCostQuery = @"select BookingCharge,TravelAllowance,ServiceCharge,TotalAmount as TotalBookingCost from BookingPricing where bookingid = @BookingID";
                var bookingCost = await connection.QueryFirstOrDefaultAsync<BookingCostDetailDTO>(bookingCostQuery, new { BookingID = booking.BookingID });

                booking.bookingCostDetails = bookingCost;
            }
            return bookingresults;
        }

        public async Task<IEnumerable<GetBookingDetailsDTO>> GetBookingsByUserID(string? userId)
        {
            var sql = @"select b.bookingid,b.customername,b.CustomerID,b.email,b.phone,b.issue,b.bookingstatus,b.paymentstatus,b.createdat,a.street,a.city,a.pincode,d.devicename,d.devicetype,s.servicename,u.name as technicianName,u.phone as technicianPhone,t.technicianId  from 
Bookings b join 
Addresses a on b.addressid=a.addressid join 
Devices d on d.deviceid=b.deviceid join 
Services s on s.serviceid=b.serviceid join 
Technicians t on t.technicianid=b.technicianid join 
Users u on t.userid=u.userid where b.customerid=@UserID";
            using var connection = context.CreateConnection();
            var bookingresults=await connection.QueryAsync<GetBookingDetailsDTO>(sql, new { UserID = userId });
            foreach (var booking in bookingresults)
            {
                if (booking.BookingStatus == "Started" || booking.BookingStatus == "Completed")
                {

                    var spareQuery = @"SELECT ID, SpareName, Price, Qty, TotalCost FROM UsedSpares WHERE bookingid = @BookingID";
                    var spares = await connection.QueryAsync<GetSpareDTO>(spareQuery, new { BookingID = booking.BookingID });
                    booking.Spares = spares;
                    booking.SparesTotal = spares.Sum(x => x.TotalCost);
                }
                var bookingCostQuery = @"select BookingCharge,TravelAllowance,ServiceCharge,TotalAmount as TotalBookingCost from BookingPricing where bookingid = @BookingID";
                var bookingCost = await connection.QueryFirstOrDefaultAsync<BookingCostDetailDTO>(bookingCostQuery, new { BookingID = booking.BookingID });

                booking.bookingCostDetails = bookingCost;

            }
            return bookingresults;
        }

        public async Task<GetBookingDetailsDTO> GetBookingByID(Guid? bookingId)
        {
            var sql = @"select b.bookingid,b.customername,b.CustomerID,b.email,b.phone,b.issue,b.bookingstatus,b.paymentstatus,b.createdat,a.street,a.city,a.pincode,d.devicename,d.devicetype,s.servicename,u.name as technicianName,u.phone as technicianPhone,t.technicianId  from Bookings b join 
Addresses a on b.addressid=a.addressid join 
Devices d on d.deviceid=b.deviceid join 
Services s on s.serviceid=b.serviceid join 
Technicians t on t.technicianid=b.technicianid join 
Users u on t.userid=u.userid  where b.bookingid=@BookingID";

            var spareQuery = @"SELECT ID, SpareName, Price, Qty, TotalCost FROM UsedSpares WHERE bookingid = @BookingID";

            using var connection = context.CreateConnection();
            var spares=await connection.QueryAsync<GetSpareDTO>(spareQuery, new { BookingID = bookingId });
            var bookingresult= await connection.QueryFirstOrDefaultAsync<GetBookingDetailsDTO>(sql, new { BookingID = bookingId });
            bookingresult.Spares = spares;
            bookingresult.SparesTotal = spares.Sum(x => x.TotalCost);
            if (bookingresult.BookingStatus == "Completed")
            {
                var customerRatingQuery = @"select rating from Ratings where bookingid = @BookingID";
                var rating = await connection.QueryFirstOrDefaultAsync<int>(customerRatingQuery, new { BookingID = bookingresult.BookingID });
                bookingresult.CutomerRating = rating;
            }
            var bookingCostQuery = @"select BookingCharge,TravelAllowance,ServiceCharge,TotalAmount as TotalBookingCost from BookingPricing where bookingid = @BookingID";
            var bookingCost = await connection.QueryFirstOrDefaultAsync<BookingCostDetailDTO>(bookingCostQuery, new { BookingID = bookingresult.BookingID });

            bookingresult.bookingCostDetails = bookingCost;
            return bookingresult;
        }

        public async Task<IEnumerable<GetBookingDetailsDTO>> GetBookingsByStatus(ServiceStatus? status,string? searchString)
        {
            var sql = @"select b.bookingid,b.customername,b.CustomerID,b.email,b.phone,b.issue,b.bookingstatus,b.paymentstatus,b.createdat,a.street,a.city,a.pincode,d.devicename,d.devicetype,s.servicename,u.name as technicianName,u.phone as technicianPhone,t.technicianId  from Bookings b join 
Addresses a on b.addressid=a.addressid join 
Devices d on d.deviceid=b.deviceid join 
Services s on s.serviceid=b.serviceid join 
Technicians t on t.technicianid=b.technicianid join
Users u on t.userid=u.userid  where b.bookingstatus=@Status and (@SearchString IS NULL OR 
       d.devicename LIKE CONCAT('%', @SearchString, '%') OR 
       b.customername LIKE CONCAT('%', @SearchString, '%') OR 
       s.servicename LIKE CONCAT('%', @SearchString, '%'))
";
            if (string.IsNullOrWhiteSpace(searchString))
            {
                searchString = null;
            }

            using var connection = context.CreateConnection();
            var bookingresults= await connection.QueryAsync<GetBookingDetailsDTO>(sql, new { Status = status.ToString(), SearchString= searchString });
            foreach (var booking in bookingresults) {
                if (booking.BookingStatus == "Started" || booking.BookingStatus == "Completed")
                {
                    var spareQuery = @"SELECT ID, SpareName, Price, Qty, TotalCost FROM UsedSpares WHERE bookingid = @BookingID";
                    var spares = await connection.QueryAsync<GetSpareDTO>(spareQuery, new { BookingID = booking.BookingID });
                    booking.Spares = spares;
                    booking.SparesTotal = spares.Sum(x => x.TotalCost);
                }
                var bookingCostQuery = @"select BookingCharge,TravelAllowance,ServiceCharge,TotalAmount as TotalBookingCost from BookingPricing where bookingid = @BookingID";
                var bookingCost = await connection.QueryFirstOrDefaultAsync<BookingCostDetailDTO>(bookingCostQuery, new { BookingID = booking.BookingID });

                booking.bookingCostDetails = bookingCost;
            }
            return bookingresults;
        }

        public async Task<double> GetBookingDistance(Guid technicianId, Guid addressId) {

            var sql = @"SELECT 
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
            JOIN Addresses a ON a.AddressID = @AddressID where t.TechnicianID = @TechnicianID";

            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<double>(sql, new { AddressID= addressId, TechnicianID= technicianId });
        }

        public async Task<double> GetServiceCharge(Guid serviceid) {
            var sql = "select Price from Services where serviceid=@ServiceID";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<double>(sql, new { ServiceID= serviceid });
        }


        public async Task<IEnumerable<GetBookingDetailsDTO>> GetBookingsByTechnicianIdAndStatus(Guid? technicianId, ServiceStatus? status)
        {
            string sql;

            if (status == ServiceStatus.InProgress)
            {
                sql = @"
            SELECT b.bookingid, b.customername,b.CustomerID, b.email, b.phone, b.issue, b.bookingstatus, b.paymentstatus, b.createdat,
                   a.street, a.city, a.pincode, d.devicename, d.devicetype, s.servicename, u.name AS technicianName, 
                   u.phone AS technicianPhone, t.technicianId 
            FROM Bookings b 
            JOIN Addresses a ON b.addressid = a.addressid 
            JOIN Devices d ON d.deviceid = b.deviceid 
            JOIN Services s ON s.serviceid = b.serviceid 
            JOIN Technicians t ON t.technicianid = b.technicianid 
            JOIN Users u ON t.userid = u.userid 
            WHERE b.technicianid = @TechnicianId 
              AND b.bookingstatus NOT IN ('Completed', 'Rejected', 'Assigned','Reassigned')";
            }
            else if (status == ServiceStatus.Assigned)
            {
                sql = @"
        SELECT b.bookingid, b.customername,b.CustomerID, b.email, b.phone, b.issue, b.bookingstatus, b.paymentstatus, b.createdat,
               a.street, a.city, a.pincode, d.devicename, d.devicetype, s.servicename, u.name AS technicianName, 
               u.phone AS technicianPhone, t.technicianId 
        FROM Bookings b 
        JOIN Addresses a ON b.addressid = a.addressid 
        JOIN Devices d ON d.deviceid = b.deviceid 
        JOIN Services s ON s.serviceid = b.serviceid 
        JOIN Technicians t ON t.technicianid = b.technicianid 
        JOIN Users u ON t.userid = u.userid 
        WHERE b.bookingstatus IN ('Assigned', 'Reassigned') 
          AND b.technicianid = @TechnicianId";
            }
            else
            {
                sql = @"
            SELECT b.bookingid, b.customername,b.CustomerID, b.email, b.phone, b.issue, b.bookingstatus, b.paymentstatus, b.createdat,
                   a.street, a.city, a.pincode, d.devicename, d.devicetype, s.servicename, u.name AS technicianName, 
                   u.phone AS technicianPhone, t.technicianId 
            FROM Bookings b 
            JOIN Addresses a ON b.addressid = a.addressid 
            JOIN Devices d ON d.deviceid = b.deviceid 
            JOIN Services s ON s.serviceid = b.serviceid 
            JOIN Technicians t ON t.technicianid = b.technicianid 
            JOIN Users u ON t.userid = u.userid 
            WHERE b.bookingstatus = @Status 
              AND b.technicianid = @TechnicianId";
            }

            using var connection = context.CreateConnection();
            var bookingResults = await connection.QueryAsync<GetBookingDetailsDTO>(sql, new { Status = status.ToString(), TechnicianId = technicianId });

            foreach (var booking in bookingResults)
            {
                if (booking.BookingStatus == "Started" || booking.BookingStatus == "Completed")
                {
                    var spareQuery = @"SELECT ID, SpareName, Price, Qty, TotalCost FROM UsedSpares WHERE bookingid = @BookingID";
                    var spares = await connection.QueryAsync<GetSpareDTO>(spareQuery, new { BookingID = booking.BookingID });
                    booking.Spares = spares;
                    booking.SparesTotal = spares.Sum(x => x.TotalCost);
                }
                var bookingCostQuery = @"select BookingCharge,TravelAllowance,ServiceCharge,TotalAmount as TotalBookingCost from BookingPricing where bookingid = @BookingID";
                var bookingCost = await connection.QueryFirstOrDefaultAsync<BookingCostDetailDTO>(bookingCostQuery, new { BookingID = booking.BookingID });

                booking.bookingCostDetails = bookingCost;
            }

            return bookingResults;
        }



        public async Task<IEnumerable<GetBookingDetailsDTO>> GetBookingsInProgress(Guid? technicianId, ServiceStatus? status, string? searchString) {
            var sql = @"
        SELECT b.bookingid, b.customername,b.CustomerID, b.email, b.phone, b.issue, b.bookingstatus, b.paymentstatus, b.createdat,
               a.street, a.city, a.pincode,
               d.devicename, d.devicetype,
               s.servicename,
               u.name AS technicianName, u.phone AS technicianPhone,t.technicianId
        FROM Bookings b
        JOIN Addresses a ON b.addressid = a.addressid
        JOIN Devices d ON d.deviceid = b.deviceid
        JOIN Services s ON s.serviceid = b.serviceid
        JOIN Technicians t ON t.technicianid = b.technicianid
        JOIN Users u ON t.userid = u.userid
        WHERE b.bookingstatus NOT IN ('Completed', 'Assigned','Rejected','Reassigned')
  AND (@SearchString IS NULL OR 
       d.devicename LIKE CONCAT('%', @SearchString, '%') OR 
       b.customername LIKE CONCAT('%', @SearchString, '%') OR 
       s.servicename LIKE CONCAT('%', @SearchString, '%'))
";

            if (technicianId != null && technicianId != Guid.Empty)
            {
                sql += " AND b.technicianid = @TechnicianId";
            }

            using var connection = context.CreateConnection();
            var bookingresults = await connection.QueryAsync<GetBookingDetailsDTO>(sql, new { Status = status.ToString(), TechnicianId = technicianId, SearchString= searchString });
            foreach (var booking in bookingresults)
            {
                if (booking.BookingStatus == "Started")
                {
                    var spareQuery = @"SELECT ID, SpareName, Price, Qty, TotalCost FROM UsedSpares WHERE bookingid = @BookingID";
                    var spares = await connection.QueryAsync<GetSpareDTO>(spareQuery, new { BookingID = booking.BookingID });
                    booking.Spares = spares;
                    booking.SparesTotal = spares.Sum(x => x.TotalCost);
                }
                var bookingCostQuery = @"select BookingCharge,TravelAllowance,ServiceCharge,TotalAmount as TotalBookingCost from BookingPricing where bookingid = @BookingID";
                var bookingCost = await connection.QueryFirstOrDefaultAsync<BookingCostDetailDTO>(bookingCostQuery, new { BookingID = booking.BookingID });

                booking.bookingCostDetails = bookingCost;
            }
            return bookingresults;
        }



        public async Task<int> UpdatePayment(Guid bookingId) {
            var sql = "update Bookings set PaymentStatus='Paid' where BookingId=@BookingId";
            using var connection = context.CreateConnection();
            return await connection.ExecuteAsync(sql, new { BookingId = bookingId });

        }
    }

}

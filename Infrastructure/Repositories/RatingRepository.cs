using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Dapper;
using MobileMend.Domain.Entities;
using MobileMend.Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class RatingRepository:IRatingRepository
    {
        private readonly DapperContext context;
        public RatingRepository(DapperContext _context) {
            context = _context;
        }
        public async Task<int> AddOrUpdateRating(RatingCreateDTO newrating)
        {
            var checkRatingSql = "SELECT COUNT(*) FROM Ratings WHERE BookingID = @BookingID";
            var insertSql = "INSERT INTO Ratings (RatingID, BookingID, Rating, ReviewText, TechnicianID) VALUES (UUID(), @BookingID, @Rating, @ReviewText, @TechnicianID)";
            var updateSql = "UPDATE Ratings SET Rating = @Rating, ReviewText = @ReviewText WHERE BookingID = @BookingID";
            var updateAverageRatingSql = @"UPDATE Technicians SET AverageRating = (SELECT AVG(Rating) FROM Ratings WHERE TechnicianId = @TechnicianId) WHERE TechnicianId = @TechnicianId";

            using var connection = context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                int count = await connection.ExecuteScalarAsync<int>(checkRatingSql, new { BookingID = newrating.BookingID }, transaction);

                if (count > 0)
                {
                    await connection.ExecuteAsync(updateSql, new
                    {
                        BookingID = newrating.BookingID,
                        Rating = newrating.RatingNo,
                        ReviewText = newrating.ReviewText
                    }, transaction);
                }
                else
                {
                    await connection.ExecuteAsync(insertSql, new
                    {
                        BookingID = newrating.BookingID,
                        Rating = newrating.RatingNo,
                        ReviewText = newrating.ReviewText,
                        TechnicianID = newrating.TechnicianID
                    }, transaction);
                }

                int rowsAffected = await connection.ExecuteAsync(updateAverageRatingSql, new { TechnicianId = newrating.TechnicianID }, transaction);

                transaction.Commit();
                return rowsAffected;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }



        public async Task<int> HideReview(Guid reviewId) {
            var sql = "update Ratings set isHidden=True where ratingid=@RatingId";
            using var connection = context.CreateConnection();
            int rowsaffected = await connection.ExecuteAsync(sql, new { RatingId=reviewId});
            return rowsaffected;
        }


        public async Task<string> GetStatus(Guid bookingId) {
            var sql = "select BookingStatus from Bookings where BookingId=@BookingId";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<string>(sql, new { BookingId= bookingId });
            
        }
    }
}

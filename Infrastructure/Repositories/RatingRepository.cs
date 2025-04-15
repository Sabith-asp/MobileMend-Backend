using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Dapper;
using MobileMend.Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class RatingRepository:IRatingRepository
    {
        private readonly DapperContext context;
        public RatingRepository(DapperContext _context) {
            context = _context;
        }
        public async Task<int> AddRating(RatingCreateDTO newrating) {
            var sql = "insert into ratings (RatingID,BookingID,Rating,ReviewText,TechnicianID) values (UUID(),@BookingID,@Rating,@ReviewText,@TechnicianID)";
            using var connection= context.CreateConnection();
            int rowsaffected = await connection.ExecuteAsync(sql, new {BookingID=newrating.BookingID,Rating=newrating.RatingNo,ReviewText=newrating.ReviewText,TechnicianID=newrating.TechnicianID });
            return rowsaffected;
        }
    }
}

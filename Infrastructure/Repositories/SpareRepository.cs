using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Dapper;
using MobileMend.Application.DTOs;
using MobileMend.Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class SpareRepository:ISpareRepository
    {
        private readonly DapperContext context;
        public SpareRepository(DapperContext _context) {
        context = _context;
        }
        public async Task<int> AddSpare(SpareCreateDTO newSpare,Guid TechnicianID) {
            var sql = "insert into usedspares (Id,SpareName,Price,Qty,BookingID,AddedBy) values (UUID(),@SpairName,@Price,@Qty,@BookingID,@AddedBy)";
            using var connection= context.CreateConnection();
           var rowsaffected= await connection.ExecuteAsync(sql, new { SpairName=newSpare.SpareName,Price=newSpare.Price,Qty=newSpare.Qty,BookingID=newSpare.BookingID,AddedBy= TechnicianID });
            return rowsaffected;
        }
    }
}

using System;
using Dapper;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Repositories;
using MobileMend.Domain.Entities;
using MobileMend.Infrastructure.Data;

namespace MobileMend.Infrastructure.Repositories
{
    public class AddressRepository:IAddressRepository
    {
        private readonly DapperContext context;
        public AddressRepository(DapperContext _context) { 
        context=_context;
                }
        public async Task<int> AddAddress(Guid userid,AddressCreateDTO newaddress) {

            var sql = "insert into addresses (AddressID,UserID,AddressDetail,City,Pincode,Longitude,Latitude,State,Street) values (UUID(),@UserID,@AddressDetail,@City,@Pincode,@Longitude,@Latitude,@State,@Street)";
            using var connection =context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { UserID = userid, AddressDetail = newaddress.AddressDetail, City = newaddress.City, Pincode = newaddress.Pincode, Longitude = newaddress.Longitude, Latitude = newaddress.Latitude,State=newaddress.State,Street=newaddress.Street });
            return rowsaffected;
        }


        public async Task<int> RemoveAddress(Guid addressid)
        {

            var sql = "update addresses set IsDeleted= not isdeleted where AddressID=@AddressID";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { AddressID=addressid});
            return rowsaffected;
        }

        public async Task<IEnumerable<Address>> GetAddress(Guid userid)
        {

            var sql = "select * from addresses where UserID=@UserID";
            using var connection = context.CreateConnection();
            return await connection.QueryAsync<Address>(sql, new { UserID = userid });
        }





    }
}

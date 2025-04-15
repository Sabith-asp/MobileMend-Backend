using Dapper;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Repositories;
using MobileMend.Domain.Entities;
using MobileMend.Infrastructure.Data;

namespace MobileMend.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DapperContext context;
        public AuthRepository(DapperContext _context)
        {
            context = _context;
        }
        public async Task<User> GetByUserName(string username)
        {
            var sql = "Select * from users where username=@UserName";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserName = username });
        }
        public async Task<User> GetByEmail(string email)
        {
            var sql = "Select * from users where email=@Email";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task Register(RegisterDTO regdata)
        {
            var sql = "insert into Users (UserID, UserName, Email, Name, Phone, PasswordHash,CreatedAt) values  (UUID(),@UserName,@Email,@Name,@Phone,@PasswordHash,NOW())";
                        using var connection = context.CreateConnection();
            await connection.ExecuteAsync(sql, new { Username = regdata.UserName, Email = regdata.Email, Name = regdata.Name, Phone = regdata.Phone, PasswordHash = regdata.Password });
        }

        public async Task<User> CheckRefreshToken(Guid userid)
        {
            var sql = "select * from users where userid=@UserID";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserID = userid });
        }

        public async Task UpdateRefreshToken(Guid userid, string newrefreshtoken)
        {
            var sql = "update users set refreshtoken=@NewRefreshToken,refreshtokenexpiry=@RefreshExpiry where userid=@UserID";
            using var connection = context.CreateConnection();
            await connection.ExecuteAsync(sql, new { UserId = userid, NewRefreshToken = newrefreshtoken, RefreshExpiry = DateTime.Now.AddDays(7) });
        }
    }
}

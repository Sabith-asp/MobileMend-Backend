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
            var sql = "Select * from Users where username=@UserName";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserName = username });
        }
        public async Task<User> GetByEmail(string email)
        {
            var sql = "Select * from Users where email=@Email";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task Register(RegisterDTO regdata, Guid userId)
        {
            var sql = "insert into Users (UserID, UserName, Email, Name, Phone, PasswordHash,CreatedAt) values  (@UserId,@UserName,@Email,@Name,@Phone,@PasswordHash,NOW())";
                        using var connection = context.CreateConnection();
            await connection.ExecuteAsync(sql, new { UserId= userId, Username = regdata.UserName, Email = regdata.Email, Name = regdata.Name, Phone = regdata.Phone, PasswordHash = regdata.Password });
        }

        public async Task<User> CheckRefreshToken(Guid userid)
        {
            var sql = "select * from Users where userid=@UserID";
            using var connection = context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserID = userid });
        }

        public async Task UpdateRefreshToken(Guid userid, string newrefreshtoken)
        {
            var sql = "update Users set refreshtoken=@NewRefreshToken,refreshtokenexpiry=@RefreshExpiry where userid=@UserID";
            using var connection = context.CreateConnection();
            await connection.ExecuteAsync(sql, new { UserId = userid, NewRefreshToken = newrefreshtoken, RefreshExpiry = DateTime.Now.AddDays(7) });
        }

        public async Task<Guid> GetTechnicianIdByUserId(string userid)
        {
            var sql = "select TechnicianID from Technicians where userid=@UserId";
            using var connection = context.CreateConnection();
            var id= await connection.QueryFirstOrDefaultAsync<Guid>(sql, new { UserId = userid });
            return id;
        }

        public async Task<string> GetTechnicianStatus(Guid technicianId)
        {
            var sql = @"select isAvailable from Technicians where technicianId=@TechnicianId";
            using var connection = context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<string>(sql, new { TechnicianId = technicianId });

        }

        public async Task updateEmailVerifyToken(Guid userId, string token) {
            var sql = @"update Users set EmailVerificationToken=@EmailVerificationToken where userid=@UserId";
            using var connection = context.CreateConnection();

            await connection.QueryFirstOrDefaultAsync<string>(sql, new { UserId = userId, EmailVerificationToken=token });
        }
    }
}

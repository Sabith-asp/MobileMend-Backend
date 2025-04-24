using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using MobileMend.Domain.Entities;
using MobileMend.Infrastructure.Data;
using Dapper;

namespace Infrastructure.Repositories
{
    public class EmailRepository:IEmailRepository
    {
        private readonly DapperContext context;
        public EmailRepository(DapperContext _context)
        {

            context = _context;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            using var connection=context.CreateConnection();
            var sql = @"SELECT * FROM Users WHERE email = @Email";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task UpdateConfirmation(Guid userId)
        {
            var sql = @"UPDATE Users SET EmailConfirmed = TRUE,EmailVerificationToken=NULL WHERE UserId = @UserId";
            using var connection = context.CreateConnection();

            await connection.ExecuteAsync(sql, new { UserId = userId });
        }
    }
}

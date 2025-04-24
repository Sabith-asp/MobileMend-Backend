using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileMend.Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IEmailRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateConfirmation(Guid userId);
    }
}

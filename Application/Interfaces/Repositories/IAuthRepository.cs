using MobileMend.Application.DTOs;
using MobileMend.Domain.Entities;

namespace MobileMend.Application.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<User> GetByUserName(string email);
        Task Register(RegisterDTO regdata,Guid userId);

        Task<User> CheckRefreshToken(Guid userid);
        Task UpdateRefreshToken(Guid userid,string newrefreshtoken);
        Task<User> GetByEmail(string email);

        Task<Guid> GetTechnicianIdByUserId(string userid);

        Task<string> GetTechnicianStatus(Guid technicianId);

        Task updateEmailVerifyToken(Guid userId,string token);




    }
}
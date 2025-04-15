using MobileMend.Application.DTOs;

namespace MobileMend.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ResponseDTO<object>> Register(RegisterDTO regdata);

        Task<ResponseDTO<object>> Login(LoginDTO logindata);

        Task<ResponseDTO<object>> RefreshAccessToken(Guid userid);
    }
}

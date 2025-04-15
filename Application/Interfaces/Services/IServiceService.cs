
using MobileMend.Application.DTOs;

namespace MobileMend.Application.Interfaces.Services
{
    public interface IServiceService
    {
        Task<ResponseDTO<object>> AddService(ServiceCreateDTO newservice);
        Task<ResponseDTO<object>> UpdateService(Guid serviceid, ServiceCreateDTO servicedata);
        Task<ResponseDTO<object>> DeleteService(Guid serviceid);
        Task<ResponseDTO<IEnumerable<object>>> GetAllService(bool isAdmin);
    }
}

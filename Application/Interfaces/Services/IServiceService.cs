
using Application.DTOs;
using MobileMend.Application.DTOs;

namespace MobileMend.Application.Interfaces.Services
{
    public interface IServiceService
    {
        Task<ResponseDTO<object>> AddService(ServiceCreateDTO newservice);
        Task<ResponseDTO<object>> UpdateService(ServiceCreateDTO servicedata);
        Task<ResponseDTO<object>> DeleteService(Guid serviceid);
        Task<ResponseDTO<IEnumerable<ServiceDTO>>> GetService(ServiceFilterDTO filterData);
    }
}

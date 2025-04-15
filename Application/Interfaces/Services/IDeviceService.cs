
using MobileMend.Application.DTOs;

namespace MobileMend.Application.Interfaces.Services
{
    public interface IDeviceService
    {
        Task<ResponseDTO<object>> AddDevice(DeviceCreateDTO newdevice);
        Task<ResponseDTO<object>> UpdateDevice(Guid deviceid,DeviceCreateDTO newdevice);
        Task<ResponseDTO<object>> DeleteDevice(Guid deviceid);
        Task<ResponseDTO<IEnumerable<object>>> GetAllDevice(bool isAdmin);
    }
}

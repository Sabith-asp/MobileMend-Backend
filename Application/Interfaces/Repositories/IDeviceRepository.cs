using Application.DTOs;
using MobileMend.Application.DTOs;
using MobileMend.Domain.Entities;

namespace MobileMend.Application.Interfaces.Repositories
{
    public interface IDeviceRepository
    {
        Task<int> AddDevice(DeviceCreateDTO newdevice);
        Task<int> UpdateDevice(Guid? deviceid,DeviceCreateDTO newdevice);
        Task<int> DeleteDevice(Guid deviceid);

        Task<IEnumerable<Device>> GetDevice(DeviceFilterDTO filter);

    }
}

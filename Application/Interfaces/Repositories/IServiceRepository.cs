
using Application.DTOs;
using MobileMend.Application.DTOs;
using MobileMend.Domain.Entities;

namespace MobileMend.Application.Interfaces.Repositories
{
    public interface IServiceRepository
    {

        Task<int> AddService(ServiceCreateDTO newservice);
        Task<int> UpdateService(Guid? Serviceid, ServiceCreateDTO servicedata);
        Task<int> DeleteService(Guid serviceid);

        Task<IEnumerable<Service>> GetServices(ServiceFilterDTO filterData);

    }
}

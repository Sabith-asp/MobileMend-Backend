using MobileMend.Application.DTOs;
using MobileMend.Domain.Entities;

namespace MobileMend.Application.Interfaces.Repositories
{
    public interface IAddressRepository
    {
        Task<int> AddAddress(Guid userid,AddressCreateDTO newaddress);
        Task<int> RemoveAddress(Guid addressid);
        Task<IEnumerable<Address>> GetAddress(Guid userid);
    }
}

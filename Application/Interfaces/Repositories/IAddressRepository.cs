using MobileMend.Application.DTOs;
using MobileMend.Domain.Entities;

namespace MobileMend.Application.Interfaces.Repositories
{
    public interface IAddressRepository
    {
        Task<int> AddAddress(string userid,AddressCreateDTO newaddress);
        Task<int> RemoveAddress(Guid addressid);
        Task<IEnumerable<Address>> GetAddress(string userid);
    }
}

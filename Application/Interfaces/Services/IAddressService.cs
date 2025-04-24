
using MobileMend.Application.DTOs;

namespace MobileMend.Application.Interfaces.Services
{
    public interface IAddressService
    {
        Task<ResponseDTO<object>> AddAddress(string userid,AddressCreateDTO newaddress);
        Task<ResponseDTO<object>> RemoveAddress(Guid addressid);
        Task<ResponseDTO<IEnumerable<AddressDTO>>> GetAddress(string userid);

    }
}

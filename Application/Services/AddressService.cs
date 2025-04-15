using AutoMapper;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Repositories;
using MobileMend.Application.Interfaces.Services;

namespace MobileMend.Application.Services
{
    public class AddressService: IAddressService
    {
        private readonly IAddressRepository addressRepo;
        private readonly IMapper mapper;

        public AddressService(IAddressRepository _addressRepo,IMapper _mapper) {
            addressRepo = _addressRepo;
            mapper=_mapper;
        }
        public async Task<ResponseDTO<object>> AddAddress(Guid userid,AddressCreateDTO newaddress) {
            try {
            
                var result=await addressRepo.AddAddress(userid,newaddress);
                if (result < 1) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in adding address" };
                }

                return new ResponseDTO<object> { StatusCode = 200, Message = "Address added" };
            } catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }
        }


        public async Task<ResponseDTO<object>> RemoveAddress(Guid addressid)
        {
            try
            {

                var result = await addressRepo.RemoveAddress(addressid);
                if (result < 1)
                {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in removing address" };
                }

                return new ResponseDTO<object> { StatusCode = 200, Message = "Address removed" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }
        }

        public async Task<ResponseDTO<IEnumerable<AddressDTO>>> GetAddress(Guid userid)
        {
            try
            {

                var result = await addressRepo.GetAddress( userid);

                if (result==null && result.Any()) {
                    return new ResponseDTO<IEnumerable<AddressDTO>> { StatusCode = 400, Message = "Error in retrieving Address" };
                
                }
                var data=mapper.Map<IEnumerable<AddressDTO>>(result);
                return new ResponseDTO<IEnumerable<AddressDTO>> { StatusCode = 200, Message = "Address retrieved", Data=data };

            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<AddressDTO>> { StatusCode = 500, Error = ex.Message };
            }
        }

    }
}

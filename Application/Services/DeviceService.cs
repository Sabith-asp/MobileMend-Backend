using Application.DTOs;
using AutoMapper;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Repositories;
using MobileMend.Application.Interfaces.Services;

namespace MobileMend.Application.Services
{
    public class DeviceService:IDeviceService
    {
        private readonly IDeviceRepository devicerepo;
        private readonly IMapper mapper;
        public DeviceService(IDeviceRepository _devicerepo,IMapper _mapper) { 
            devicerepo = _devicerepo;
            mapper = _mapper;
        }

        public async Task<ResponseDTO<IEnumerable<object>>> GetDevice(bool isAdmin,DeviceFilterDTO filter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filter.Search))
                {
                    filter.Search = null;
                }

                var result = await devicerepo.GetDevice(filter);
                if (isAdmin) {

                    return new ResponseDTO<IEnumerable<object>> { StatusCode = 200, Message = "Devices retrieved",Data=result };

                }
                var data=mapper.Map<IEnumerable<DeviceDTO>>(result.Where(device=>device.isDeleted==false));
                return new ResponseDTO<IEnumerable<object>> { StatusCode = 200, Message = "Devices retrieved", Data = data };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<object>> { StatusCode = 500, Error = ex.Message };
            }

        }
        public async Task<ResponseDTO<object>> AddDevice(DeviceCreateDTO newdevice)
        {
            try {

                var result= await devicerepo.AddDevice(newdevice);
                if (result < 1) { return new ResponseDTO<object> {StatusCode=400,Message="Error in adding device" }; }
                return new ResponseDTO<object> { StatusCode = 200, Message = "Device added" };
            
            } catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }

        }

        public async Task<ResponseDTO<object>> UpdateDevice(Guid? deviceid,DeviceCreateDTO newdevice)
        {
            try
            {

                var result = await devicerepo.UpdateDevice(deviceid,newdevice);
                if (result < 1) { return new ResponseDTO<object> { StatusCode = 400, Message = "Error in updating device" }; }
                return new ResponseDTO<object> { StatusCode = 200, Message = "Device updated" };

            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }

        }


        public async Task<ResponseDTO<object>> DeleteDevice(Guid deviceid)
        {
            try
            {

                var result = await devicerepo.DeleteDevice(deviceid);
                if (result < 1) { return new ResponseDTO<object> { StatusCode = 400, Message = "Error in deleting device" }; }
                return new ResponseDTO<object> { StatusCode = 200, Message = "Device deleted" };

            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }

        }
    }
}

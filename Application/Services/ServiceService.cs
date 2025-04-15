using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Repositories;
using MobileMend.Application.Interfaces.Services;

namespace MobileMend.Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository servicerepo;
        private readonly IMapper mapper;
        public ServiceService(IServiceRepository _servicerepo, IMapper _mapper)
        {
            servicerepo = _servicerepo;
            mapper = _mapper;
        }
        public async Task<ResponseDTO<object>> AddService(ServiceCreateDTO newservice)
        {
            try
            {

                var rowsaffected = await servicerepo.AddService(newservice);
                if (rowsaffected < 1)
                {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in adding service" };
                }
                return new ResponseDTO<object> { StatusCode = 200, Message = "Service added successfully" };

            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }

        }

        public async Task<ResponseDTO<object>> UpdateService(Guid serviceid, ServiceCreateDTO servicedata)
        {
            try {
                var result=await servicerepo.UpdateService(serviceid, servicedata);
                if (result < 1) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in updating service" };
                }
                return new ResponseDTO<object> { StatusCode = 200, Message = "Service updated" };
            } catch (Exception ex) {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };

            }
        }

        public async Task<ResponseDTO<object>> DeleteService(Guid serviceid)
        {
            try {
                var result=await servicerepo.DeleteService(serviceid);
                if (result<1) { return new ResponseDTO<object> { StatusCode = 400, Message = "Error in updating service" }; };
                return new ResponseDTO<object> { StatusCode = 200, Message="Service deleted" };
            } catch (Exception ex) {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }
        }

        public async Task<ResponseDTO<IEnumerable<object>>> GetAllService(bool isAdmin) {
            try {
                var services=await servicerepo.GetAllServices();
                if (isAdmin)
                {
                    return new ResponseDTO<IEnumerable<object>> { StatusCode = 200, Message = "services retrieved", Data = services };
                }
                else {
                    var data=mapper.Map<IEnumerable<ServiceDTO>>(services.Where(service=>service.IsDeleted==false));
                    return new ResponseDTO<IEnumerable<object>> { StatusCode = 200, Message = "services retrieved", Data=data };
                    }
            } catch (Exception ex) {
                return new ResponseDTO<IEnumerable<object>> { StatusCode = 500, Error = ex.Message };

            }

        }
    }
}

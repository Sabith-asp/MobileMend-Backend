using Application.DTOs;
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

        public async Task<ResponseDTO<object>> UpdateService(ServiceCreateDTO servicedata)
        {
            try {
                var result=await servicerepo.UpdateService(servicedata.ServiceId, servicedata);
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

        public async Task<ResponseDTO<IEnumerable<ServiceDTO>>> GetService(ServiceFilterDTO filter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filter.Search))
                {
                    filter.Search = null;
                }
                var services = await servicerepo.GetServices(filter);

                var serviceDTOs = services.Select(service => mapper.Map<ServiceDTO>(service));

                return new ResponseDTO<IEnumerable<ServiceDTO>> { StatusCode = 200, Message = "Services retrieved", Data = serviceDTOs };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<ServiceDTO>> { StatusCode = 500, Error = ex.Message };
            }
        }

    }
}

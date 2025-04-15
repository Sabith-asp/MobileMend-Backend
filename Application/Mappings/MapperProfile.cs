using Application.DTOs;
using AutoMapper;
using MobileMend.Application.DTOs;
using MobileMend.Domain.Entities;

namespace MobileMend.Application.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Service,ServiceDTO>().ReverseMap();
            CreateMap<Device, DeviceDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<TechnicianRequestCreateDTO, TechncianRequestAddDTO>();
        }
    }
}

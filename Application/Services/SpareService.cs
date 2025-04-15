using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using MobileMend.Application.DTOs;

namespace Application.Services
{
    public class SpareService:ISpareService
    {
        private readonly ISpareRepository spareRepository;
        public SpareService(ISpareRepository _spareRepository) {
            spareRepository = _spareRepository;
        }
        public async Task<ResponseDTO<object>> AddSpare(SpareCreateDTO newSpare, Guid TechnicianID) {
            try
            {

                var rowsaffected = await spareRepository.AddSpare(newSpare, TechnicianID);
                if (rowsaffected < 1)
                {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Error in adding spare" };
                }
                return new ResponseDTO<object> { StatusCode = 200, Message = "Spare added successfully" };

            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }
        }
    }
}

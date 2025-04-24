using Application.DTOs;
using CloudinaryDotNet;
using Dapper;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Repositories;
using MobileMend.Domain.Entities;
using MobileMend.Infrastructure.Data;
namespace MobileMend.Infrastructure.Repositories
{
    public class DeviceRepository:IDeviceRepository
    {
        private readonly DapperContext context;
        public DeviceRepository(DapperContext _context) { 
        
            context= _context;
        }

        public async Task<IEnumerable<Device>> GetDevice(DeviceFilterDTO filter)
        {

            var sql = @"select * from Devices where isDeleted=FALSE and  (@DeviceId IS NULL OR DeviceId = @DeviceId)
        AND (@Search IS NULL OR DeviceName LIKE CONCAT('%', @Search, '%') OR Brand LIKE CONCAT('%', @Search, '%') OR Model LIKE CONCAT('%', @Search, '%') OR DeviceType LIKE CONCAT('%', @Search, '%') OR CommonIssues LIKE CONCAT('%', @Search, '%') OR RepairableComponents LIKE CONCAT('%', @Search, '%'))";
            using var connection = context.CreateConnection();
            return await connection.QueryAsync<Device>(sql,new { DeviceId=filter.DeviceId,Search=filter.Search } );

        }

        public async Task<int> AddDevice(DeviceCreateDTO newdevice) {

            var sql = "insert into Devices (DeviceID, DeviceName, Brand, DeviceType, Model, ReleaseYear, CommonIssues, RepairableComponents) values (UUID(),@DeviceName,@Brand,@DeviceType,@Model,@ReleaseYear,@CommonIssues,@RepairableComponents)";
            using var connection=context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { DeviceName=newdevice.DeviceName, Brand = newdevice.Brand, DeviceType = newdevice.DeviceType , Model = newdevice.Model , ReleaseYear = newdevice.ReleaseYear , CommonIssues = newdevice.CommonIssues , RepairableComponents = newdevice.RepairableComponents });
            return rowsaffected;    
        
        }

        public async Task<int> UpdateDevice(Guid? deviceid,DeviceCreateDTO newdevice)
        {

            var sql = "update Devices set DeviceName=@DeviceName, Brand=@Brand, DeviceType=@DeviceType, Model=@Model, ReleaseYear=@ReleaseYear, CommonIssues=@CommonIssues, RepairableComponents=@RepairableComponents where DeviceID=@DeviceID";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { DeviceName = newdevice.DeviceName, Brand = newdevice.Brand, DeviceType = newdevice.DeviceType, Model = newdevice.Model, ReleaseYear = newdevice.ReleaseYear, CommonIssues = newdevice.CommonIssues, RepairableComponents = newdevice.RepairableComponents,DeviceID=deviceid });
            return rowsaffected;

        }


        public async Task<int> DeleteDevice(Guid deviceid)
        {

            var sql = "update Devices set IsDeleted= NOT IsDeleted where DeviceID=@DeviceID";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql,new { deviceid=deviceid});
            return rowsaffected;

        }
    }
}

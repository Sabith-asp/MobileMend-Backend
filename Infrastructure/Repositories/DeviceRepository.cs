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

        public async Task<IEnumerable<Device>> GetAllDevice(bool isAdmin)
        {

            var sql = "select * from devices";
            using var connection = context.CreateConnection();
            return await connection.QueryAsync<Device>(sql);


        }

        public async Task<int> AddDevice(DeviceCreateDTO newdevice) {

            var sql = "insert into devices (DeviceID, DeviceName, Brand, DeviceType, Model, ReleaseYear, CommonIssues, RepairableComponents) values (UUID(),@DeviceName,@Brand,@DeviceType,@Model,@ReleaseYear,@CommonIssues,@RepairableComponents)";
            using var connection=context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { DeviceName=newdevice.DeviceName, Brand = newdevice.Brand, DeviceType = newdevice.DeviceType , Model = newdevice.Model , ReleaseYear = newdevice.ReleaseYear , CommonIssues = newdevice.CommonIssues , RepairableComponents = newdevice.RepairableComponents });
            return rowsaffected;    
        
        }

        public async Task<int> UpdateDevice(Guid deviceid,DeviceCreateDTO newdevice)
        {

            var sql = "update devices set DeviceName=@DeviceName, Brand=@Brand, DeviceType=@DeviceType, Model=@Model, ReleaseYear=@ReleaseYear, CommonIssues=@CommonIssues, RepairableComponents=@RepairableComponents where DeviceID=@DeviceID";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql, new { DeviceName = newdevice.DeviceName, Brand = newdevice.Brand, DeviceType = newdevice.DeviceType, Model = newdevice.Model, ReleaseYear = newdevice.ReleaseYear, CommonIssues = newdevice.CommonIssues, RepairableComponents = newdevice.RepairableComponents,DeviceID=deviceid });
            return rowsaffected;

        }


        public async Task<int> DeleteDevice(Guid deviceid)
        {

            var sql = "update devices set IsDeleted= NOT IsDeleted where DeviceID=@DeviceID";
            using var connection = context.CreateConnection();
            var rowsaffected = await connection.ExecuteAsync(sql,new { deviceid=deviceid});
            return rowsaffected;

        }
    }
}

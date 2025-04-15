using Dapper;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Repositories;
using MobileMend.Domain.Entities;
using MobileMend.Infrastructure.Data;
using System.Linq;

namespace MobileMend.Infrastructure.Repositories
{
    public class ServiceRepository:IServiceRepository
    {
        private readonly DapperContext context;
        public ServiceRepository(DapperContext _context) {
            context = _context;
        }
        public async Task<int> AddService(ServiceCreateDTO newservice) {

            var sql = "insert into services (ServiceID, ServiceName, Description, EstimatedTime, Price,Category) values (UUID(),@ServiceName,@Description,@EstimatedTime,@Price,@Category)";

            using var connection=context.CreateConnection();
            var rowsaffected= await connection.ExecuteAsync(sql, new { ServiceName=newservice.ServiceName, Description =newservice.Description, EstimatedTime =newservice.EstimatedTime,Price=newservice.Price,Category=newservice.Category});
            return rowsaffected;
        }

        public async Task<int> UpdateService(Guid Serviceid, ServiceCreateDTO servicedata) {
            var sql= "update services set ServiceName=@ServiceName,Description=@Description,EstimatedTime=@EstimatedTime,Price=@Price,IsPopular=@IsPopular,UpdatedAt=@UpdatedAt where ServiceID=@ServiceID";
            using var conneection = context.CreateConnection();
            var rowsaffected = await conneection.ExecuteAsync(sql,new { ServiceName=servicedata.ServiceName, Description=servicedata.Description, EstimatedTime=servicedata.EstimatedTime, Price=servicedata.Price, IsPopular = servicedata.IsPopular, ServiceID = Serviceid, UpdatedAt=DateTime.UtcNow });

            return rowsaffected;
        }

        public async Task<int> DeleteService(Guid serviceid) {

            var sql = "update services set isdeleted=not isdeleted where ServiceID=@ServiceID";
            using var connection= context.CreateConnection();
            var rowsaffected=await connection.ExecuteAsync(sql, new { ServiceID=serviceid});
            return rowsaffected;
        }

        public async Task<IEnumerable<Service>> GetAllServices() {
            var sql = "select * from services";
            using var connection=context.CreateConnection();
            return await connection.QueryAsync<Service>(sql);
        }

    }
}


using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;


namespace MobileMend.Infrastructure.Data
{
    public class DapperContext
    {
        private readonly IConfiguration configuration;
        public DapperContext(IConfiguration _configuration)
        {
            configuration = _configuration;

        }

        public IDbConnection CreateConnection() => new MySqlConnection(configuration.GetConnectionString("ConnStr"));
    }
}



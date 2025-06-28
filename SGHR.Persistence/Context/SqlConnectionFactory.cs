using Microsoft.Data.SqlClient;
using SGHR.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace SGHR.Persistence.Context
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SGHR") ?? throw new InvalidOperationException("Connection string 'SGHR' no encontrada");
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}

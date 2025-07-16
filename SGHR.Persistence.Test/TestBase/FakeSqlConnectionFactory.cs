using Microsoft.Data.SqlClient;
using SGHR.Persistence.Interfaces;
using System.Data;
namespace SGHR.Persistence.Test.TestBase
{
    public class FakeSqlConnectionFactory : ISqlConnectionFactory
    {
        public SqlConnection CreateConnection() =>
            new SqlConnection("Data Source=(local);Initial Catalog=TestDB;Integrated Security=True");

        IDbConnection ISqlConnectionFactory.CreateConnection()
        {
            throw new NotImplementedException("No se puede usar SQL en pruebas de EF InMemory.");
        }
    }
}

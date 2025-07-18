using Microsoft.Data.SqlClient;

namespace SGHR.Domain.Base
{
    public interface ISqlHelper
    {
        Task<IEnumerable<T>> ExecuteReaderAsync<T>(
            string connectionString,
            string storedProcedure,
            Dictionary<string, object> parameters,
            Func<SqlDataReader, T> mapFunc);
    }
}

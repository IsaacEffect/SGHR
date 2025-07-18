using Microsoft.Data.SqlClient;
using SGHR.Domain.Base;
using System.Data;

namespace SGHR.Persistence.Base
{
    public class SqlHelperService : ISqlHelper
    {
        public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(
            string connectionString,
            string storedProcedure,
            Dictionary<string, object> parameters,
            Func<SqlDataReader, T> mapFunc)
        {
            var result = new List<T>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(storedProcedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(mapFunc(reader));
                    }
                }
            }

            return result;
        }
    }
}

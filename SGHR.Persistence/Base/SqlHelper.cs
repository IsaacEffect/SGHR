using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Base
{
    public static class SqlHelper
    {
        public static async Task<List<T>> ExecuteReaderAsync<T>(
            string connectionString,
            string storedProcedure,
            Dictionary<string, object> parameters,
            Func<SqlDataReader, T> mapFunction)
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
                        result.Add(mapFunction(reader));
                    }
                }
            }

            return result;
        }
    }

}

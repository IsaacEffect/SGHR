using Dapper;
using Microsoft.Data.SqlClient; 
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces;
using SGHR.Persistence.Interfaces.Repositories; 
using System;
using System.Data; 
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace SGHR.Persistence.Repositories
{
    public class ServicioCategoriaRepository : IServicioCategoriaRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public ServicioCategoriaRepository (ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task AgregarPrecioServicioCategoriaAsync(int servicioId, int categoriaId, decimal precio)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            parameters.Add("@IdServicio", servicioId, DbType.Int32);
            parameters.Add("@IdCategoriaHabitacion", categoriaId, DbType.Int32);
            parameters.Add("@Precio", precio, DbType.Decimal); 

            await connection.ExecuteAsync(
                "UpsertPrecioServicioCategoria", 
                parameters,
                commandType: CommandType.StoredProcedure
            );
            
        }
        public async Task ActualizarPrecioServicioCategoriaAsync(int servicioId, int categoriaId, decimal precio)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            parameters.Add("@IdServicio", servicioId, DbType.Int32);
            parameters.Add("@IdCategoriaHabitacion", categoriaId, DbType.Int32);
            parameters.Add("@Precio", precio, DbType.Decimal);

            await connection.ExecuteAsync(
                "UpsertPrecioServicioCategoria", 
                parameters,
                commandType: CommandType.StoredProcedure
            );
            
        }
        public async Task EliminarPrecioServicioCategoriaAsync(int servicioId, int categoriaId)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            parameters.Add("@IdServicio", servicioId, DbType.Int32);
            parameters.Add("@IdCategoriaHabitacion", categoriaId, DbType.Int32);

            await connection.ExecuteAsync(
                "EliminarPrecioServicioCategoria", // Nombre de tu SP para eliminar
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

using Dapper;
using SGHR.Domain.Entities.Servicios;
using SGHR.Persistence.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using System.Data;

namespace SGHR.Persistence.Repositories.Servicios
{
    public class ServicioCategoriaRepository : IServicioCategoriaRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public ServicioCategoriaRepository(ISqlConnectionFactory sqlConnectionFactory)
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
                "EliminarPrecioServicioCategoria",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<List<ServicioCategoria>> ObtenerPreciosPorServicioAsync(int servicioId)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@IdServicio", servicioId, DbType.Int32);

            
            var precios = await connection.QueryAsync<ServicioCategoria>(
                "ObtenerPreciosPorServicio", 
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return precios.AsList();
        }
        public async Task<List<ServicioCategoria>> ObtenerPreciosPorCategoriaAsync(int categoriaId)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@IdCategoriaHabitacion", categoriaId, DbType.Int32);

            var precios = await connection.QueryAsync<ServicioCategoria>( 
                "ObtenerPreciosPorCategoria",  
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return precios.AsList();
        }
        public async Task<ServicioCategoria?> ObtenerPrecioServicioCategoriaEspecificoAsync(int servicioId, int categoriaId)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@IdServicio", servicioId, DbType.Int32);
            parameters.Add("@IdCategoriaHabitacion", categoriaId, DbType.Int32);

           
            var precio = await connection.QuerySingleOrDefaultAsync<ServicioCategoria>(
                "ObtenerPrecioServicioCategoriaEspecifico",  
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return precio;
        }   
    }
}

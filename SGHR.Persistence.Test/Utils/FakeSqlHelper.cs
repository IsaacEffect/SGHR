using SGHR.Domain.Base;
using SGHR.Domain.Entities.Historial;

namespace SGHR.Persistence.Test.Utils
{
    public class FakeSqlHelper : ISqlHelper
    {
        public Task<IEnumerable<T>> ExecuteReaderAsync<T>(string connectionString, string storedProcedure, Dictionary<string, object> parameters, Func<Microsoft.Data.SqlClient.SqlDataReader, T> mapFunc)
        {
            if (storedProcedure == "dbo.ObtenerHistorialClienteFiltrado")
            {
                var lista = new List<HistorialReserva>
                {
                    new HistorialReserva { Id = 1, ClienteId = 10, Estado = "Confirmada", TipoHabitacion = "Suite" },
                    new HistorialReserva { Id = 2, ClienteId = 10, Estado = "Cancelada", TipoHabitacion = "Doble" }
                };
                return Task.FromResult(lista.Cast<T>());
            }

            if (storedProcedure == "dbo.ObtenerDetalleReservaCliente")
            {
                int idReserva = (int)parameters["@IdReserva"];
                int clienteId = (int)parameters["@ClienteId"];

                if (idReserva == 1 && clienteId == 10)
                {
                    var resultado = new List<HistorialReserva>
                    {
                        new HistorialReserva { Id = 1, ClienteId = 10, Estado = "Confirmada", TipoHabitacion = "Suite" }
                    };
                    return Task.FromResult(resultado.Cast<T>());
                }

                return Task.FromResult(Enumerable.Empty<T>());
            }


            return Task.FromResult(Enumerable.Empty<T>());
        }
    }
}

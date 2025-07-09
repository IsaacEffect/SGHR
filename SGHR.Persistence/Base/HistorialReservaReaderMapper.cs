using Microsoft.Data.SqlClient;
using SGHR.Domain.Entities.Historial;

namespace SGHR.Persistence.Base
{
    public static class HistorialReservaReaderMapper
    {
        public static HistorialReserva FromReader(SqlDataReader reader)
        {
            return new HistorialReserva
            {
                Id = Convert.ToInt32(reader["Id"]),
                FechaEntrada = Convert.ToDateTime(reader["FechaEntrada"]),
                FechaSalida = Convert.ToDateTime(reader["FechaSalida"]),
                Estado = reader["Estado"].ToString(),
                Tarifa = Convert.ToDecimal(reader["Tarifa"]),
                TipoHabitacion = reader["TipoHabitacion"].ToString(),
                ServiciosAdicionales = reader["ServiciosAdicionales"].ToString()
            };
        }
    }
}

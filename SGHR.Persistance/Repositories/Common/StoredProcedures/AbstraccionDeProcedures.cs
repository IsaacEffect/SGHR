using Microsoft.Data.SqlClient;
using SGHR.Persistance.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Infraestructure.Common.StoredProcedures
{
    public class AbstraccionDeProcedures
    {
        public async Task AbstraerStoredProcedure(string coneccion, string nombreDelProcedure, Tarifa tarifa)
        {
            using var connection = new SqlConnection(coneccion);
            using var command = new SqlCommand(nombreDelProcedure, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@IdCategoriaHabitacion", tarifa.IdCategoriaHabitacion);
            command.Parameters.AddWithValue("@FechaInicio", tarifa.FechaInicio);
            command.Parameters.AddWithValue("@FechaFin", tarifa.FechaFin);
            command.Parameters.AddWithValue("@TipoTemporada", tarifa.TipoTemporada ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Precio", tarifa.Precio);
            command.Parameters.AddWithValue("@Estado", tarifa.Estado);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}

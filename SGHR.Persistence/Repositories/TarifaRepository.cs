using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using SGHR.Domain.InterfacesRepositories;
using SGHR.Persistance.Models;
using System.Data;

namespace SGHR.Persistance.Repositories
{
    public class TarifaRepository : ITarifaRepository
    {
       private readonly string _connection;
        public TarifaRepository(IConfiguration configuration) 
        {
            _connection = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task ActualizarTarifaAsync(Tarifa tarifa)
        {
            try
            {
                using var connection = new SqlConnection(_connection);
                using var command = new SqlCommand("sp_ActualizarTarifa", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@IdTarifa", tarifa.IdTarifa);
                command.Parameters.AddWithValue("@IdCategoriaHabitacion", tarifa.IdCategoriaHabitacion);
                command.Parameters.AddWithValue("@FechaInicio", tarifa.FechaInicio);
                command.Parameters.AddWithValue("@FechaFin", tarifa.FechaFin);
                command.Parameters.AddWithValue("@TipoTemporada", tarifa.TipoTemporada);
                command.Parameters.AddWithValue("@Precio", tarifa.Precio);
                command.Parameters.AddWithValue("@Estado", tarifa.Estado);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar tarifa: {ex.Message}");
                throw;
            }
        }

        public async Task DefinirTarifaAsync(Tarifa tarifa)
        {
            try
            {
                using var connection = new SqlConnection(_connection);
                using var command = new SqlCommand("sp_DefinirTarifaPorTemporada", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@IdCategoriaHabitacion", tarifa.IdCategoriaHabitacion);
                command.Parameters.AddWithValue("@FechaInicio", tarifa.FechaInicio);
                command.Parameters.AddWithValue("@FechaFin", tarifa.FechaFin);
                command.Parameters.AddWithValue("@Precio", tarifa.Precio);
                command.Parameters.AddWithValue("@Estado", tarifa.Estado);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al definir tarifa por temporada: {ex.Message}");
                throw;
            }
        }

        public async Task DefinirTarifaPorTemporadaAsync(Tarifa tarifa)
        {
            try
            {
                using var connection = new SqlConnection(_connection);
                using var command = new SqlCommand("sp_DefinirTarifaPorTemporada", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@IdCategoriaHabitacion", tarifa.IdCategoriaHabitacion);
                command.Parameters.AddWithValue("@FechaInicio", tarifa.FechaInicio);
                command.Parameters.AddWithValue("@FechaFin", tarifa.FechaFin);
                command.Parameters.AddWithValue("@TipoTemporada", tarifa.TipoTemporada);
                command.Parameters.AddWithValue("@Precio", tarifa.Precio);
                command.Parameters.AddWithValue("@Estado", tarifa.Estado);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al definir tarifa por temporada: {ex.Message}");
                throw;
            }
        }

       
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.InterfacesRepositories;
using SGHR.Infraestructure.Common.StoredProcedures;
using SGHR.Persistance.Models;
using System.Data;

namespace SGHR.Persistance.Repositories
{
    public class TarifaRepository : ITarifaRepository
    {
       private readonly string _connection;
       private readonly AbstraccionDeProcedures _procedure;
        private readonly ILogger<TarifaRepository> _logger;
       
        public TarifaRepository(IConfiguration configuration, ILogger<TarifaRepository> logger)
        {
            _connection = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Cadena de conexión no encontrada.");

            _procedure = new AbstraccionDeProcedures();
            _logger = logger;
        }

        public async Task ActualizarTarifaAsync(Tarifa tarifa)
        {
            try
            {
                _logger.LogInformation("Iniciando actualización de tarifa con ID {IdTarifa}", tarifa.IdTarifa);

                await _procedure.AbstraerStoredProcedure(_connection, "sp_ActualizarTarifa", tarifa);

                _logger.LogInformation("Tarifa actualizada exitosamente: ID {IdTarifa}", tarifa.IdTarifa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la tarifa con ID {IdTarifa}", tarifa.IdTarifa);
                throw;
            }
        }

        public async Task DefinirTarifaAsync(Tarifa tarifa)
        {
            try
            {
                _logger.LogInformation("Iniciando definición de tarifa para categoría {IdCategoria}", tarifa.IdCategoriaHabitacion);

                await _procedure.AbstraerStoredProcedure(_connection, "sp_DefinirTarifa", tarifa);

                _logger.LogInformation("Tarifa definida exitosamente para categoría {IdCategoria}", tarifa.IdCategoriaHabitacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al definir tarifa para categoría {IdCategoria}", tarifa.IdCategoriaHabitacion);
                throw;
            }
        }

        public async Task DefinirTarifaPorTemporadaAsync(Tarifa tarifa)
        {
            try
            {
                _logger.LogInformation("Iniciando definición de tarifa por temporada para categoría {IdCategoria}", tarifa.IdCategoriaHabitacion);

                await _procedure.AbstraerStoredProcedure(_connection, "sp_DefinirTarifaPorTemporada", tarifa);

                _logger.LogInformation("Tarifa por temporada definida exitosamente para categoría {IdCategoria}", tarifa.IdCategoriaHabitacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al definir tarifa por temporada para categoría {IdCategoria}", tarifa.IdCategoriaHabitacion);
                throw;
            }
        }

        public async Task<Tarifa?> ObtenerTarifaPorID(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando búsqueda de tarifa por ID: {IdTarifa}", id);

                using var connection = new SqlConnection(_connection);
                using var command = new SqlCommand("sp_ObtenerTarifaPorId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdTarifa", id);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    _logger.LogInformation($"Tarifa encontrada con id:{id}");
                    return new Tarifa
                    {
                        IdTarifa = reader.GetInt32(reader.GetOrdinal("IdTarifa")),
                        IdCategoriaHabitacion = reader.GetInt32(reader.GetOrdinal("IdCategoriaHabitacion")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                        TipoTemporada = reader.IsDBNull(reader.GetOrdinal("TipoTemporada"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("TipoTemporada")),

                        FechaInicio = reader.IsDBNull(reader.GetOrdinal("FechaInicio"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("FechaInicio")),

                        FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("FechaFin"))
                    };

                }

                _logger.LogInformation("No se encontró tarifa con ID {IdTarifa}", id);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tarifa con ID {IdTarifa}", id);
                throw;
            }
        }
    }
}

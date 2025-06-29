using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Entities.Historial;
using SGHR.Domain.Interfaces.Repository;
using SGHR.Persistence.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories
{
    public class HistorialReservaRepository : IHistorialReservaRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<HistorialReservaRepository> _logger;

        public HistorialReservaRepository(IConfiguration configuration, ILogger<HistorialReservaRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("HotelDBConnection");
            _logger = logger;
        }

        public async Task<IEnumerable<HistorialReserva>> GetHistorialByClienteAsync(
            int clienteId,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            string estado = null,
            string tipoHabitacion = null)
        {
            try
            {
                _logger.LogInformation("Ejecutando SP dbo.ObtenerHistorialClienteFiltrado con ClienteId: {ClienteId}", clienteId);

                var parameters = new Dictionary<string, object>
                {
                    { "@ClienteId", clienteId },
                    { "@FechaInicio", fechaInicio },
                    { "@FechaFin", fechaFin },
                    { "@Estado", estado },
                    { "@TipoHabitacion", tipoHabitacion }
                };

                var historial = await SqlHelper.ExecuteReaderAsync(
                    _connectionString,
                    "dbo.ObtenerHistorialClienteFiltrado",
                    parameters,
                    reader => new HistorialReserva
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FechaEntrada = Convert.ToDateTime(reader["FechaEntrada"]),
                        FechaSalida = Convert.ToDateTime(reader["FechaSalida"]),
                        Estado = reader["Estado"].ToString(),
                        Tarifa = Convert.ToDecimal(reader["Tarifa"]),
                        TipoHabitacion = reader["TipoHabitacion"].ToString(),
                        ServiciosAdicionales = reader["ServiciosAdicionales"].ToString()
                    });

                _logger.LogInformation("Historial obtenido correctamente para el cliente con ID: {ClienteId}", clienteId);
                return historial;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial para el cliente con ID {ClienteId}", clienteId);
                throw;
            }
        }

        public async Task<HistorialReserva> GetDetalleReservaAsync(int idReserva, int clienteId)
        {
            try
            {
                _logger.LogInformation("Ejecutando SP dbo.ObtenerDetalleReservaCliente con IdReserva: {IdReserva}, ClienteId: {ClienteId}", idReserva, clienteId);

                var parameters = new Dictionary<string, object>
                {
                    { "@IdReserva", idReserva },
                    { "@ClienteId", clienteId }
                };

                var resultados = await SqlHelper.ExecuteReaderAsync(
                    _connectionString,
                    "dbo.ObtenerDetalleReservaCliente",
                    parameters,
                    reader => new HistorialReserva
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FechaEntrada = Convert.ToDateTime(reader["FechaEntrada"]),
                        FechaSalida = Convert.ToDateTime(reader["FechaSalida"]),
                        Estado = reader["Estado"].ToString(),
                        Tarifa = Convert.ToDecimal(reader["Tarifa"]),
                        TipoHabitacion = reader["TipoHabitacion"].ToString(),
                        ServiciosAdicionales = reader["ServiciosAdicionales"].ToString()
                    });

                var detalle = resultados.FirstOrDefault();

                if (detalle != null)
                {
                    _logger.LogInformation("Detalle de reserva obtenido correctamente para IdReserva: {IdReserva}", idReserva);
                    return detalle;
                }
                else
                {
                    _logger.LogWarning("No se encontró detalle de reserva para IdReserva: {IdReserva}, ClienteId: {ClienteId}", idReserva, clienteId);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el detalle de la reserva con ID {IdReserva} para el cliente {ClienteId}", idReserva, clienteId);
                throw;
            }
        }
    }
}

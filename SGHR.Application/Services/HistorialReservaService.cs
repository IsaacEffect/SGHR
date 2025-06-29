using Microsoft.Extensions.Logging;
using SGHR.Domain.Interfaces.Repository;
using SGHR.Domain.Interfaces.Service;
using SGHR.Model.Dtos;

namespace SGHR.Application.Services
{
    public class HistorialReservaService : IHistorialReservaService
    {
        private readonly IHistorialReservaRepository _repo;
        private readonly ILogger<HistorialReservaService> _logger;

        public HistorialReservaService(
            IHistorialReservaRepository repo,
            ILogger<HistorialReservaService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<HistorialReservaDto>> ObtenerHistorialAsync(
            int clienteId,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            string estado = null,
            string tipoHabitacion = null)
        {
            try
            {
                var historial = await _repo.GetHistorialByClienteAsync(
                    clienteId,
                    fechaInicio,
                    fechaFin,
                    estado,
                    tipoHabitacion);

                return historial.Select(h => new HistorialReservaDto
                {
                    Id = h.Id,
                    FechaEntrada = h.FechaEntrada,
                    FechaSalida = h.FechaSalida,
                    Estado = h.Estado,
                    Tarifa = h.Tarifa,
                    TipoHabitacion = h.TipoHabitacion,
                    ServiciosAdicionales = h.ServiciosAdicionales
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial para cliente {ClienteId}", clienteId);
                throw;
            }
        }

        public async Task<HistorialReservaDto> ObtenerDetalleAsync(int idReserva, int clienteId)
        {
            try
            {
                var reserva = await _repo.GetDetalleReservaAsync(idReserva, clienteId);

                if (reserva == null)
                    return null;

                return new HistorialReservaDto
                {
                    Id = reserva.Id,
                    FechaEntrada = reserva.FechaEntrada,
                    FechaSalida = reserva.FechaSalida,
                    Estado = reserva.Estado,
                    Tarifa = reserva.Tarifa,
                    TipoHabitacion = reserva.TipoHabitacion,
                    ServiciosAdicionales = reserva.ServiciosAdicionales
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalle de reserva {IdReserva}", idReserva);
                throw;
            }
        }
    }
}

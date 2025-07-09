using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Base.Mappers;
using SGHR.Application.Contracts.Service;
using SGHR.Application.Dtos;
using SGHR.Domain.Base;
using SGHR.Domain.Interfaces.Repository;

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

        public async Task<OperationResult<IEnumerable<HistorialReservaDto>>> ObtenerHistorialAsync(
            int clienteId,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            string? estado = null,
            string? tipoHabitacion = null)
        {
            return await ServiceExecutor.ExecuteAsync(_logger, async () =>
            {
                var historial = await _repo.GetHistorialByClienteAsync(
                    clienteId, fechaInicio, fechaFin, estado, tipoHabitacion);

                return historial.ToDtoList();
            }, "Error al obtener historial.", $"ClienteId: {clienteId}");
        }

        public async Task<OperationResult<HistorialReservaDto>> ObtenerDetalleAsync(int idReserva, int clienteId)
        {
            return await ServiceExecutor.ExecuteAsync(_logger, async () =>
            {
                var reserva = await _repo.GetDetalleReservaAsync(idReserva, clienteId);
                return reserva == null ? throw new InvalidOperationException("Reserva no encontrada o no pertenece al cliente.") : reserva.ToDto();
            }, "Error al obtener detalle de la reserva.", $"IdReserva: {idReserva}, ClienteId: {clienteId}");
        }
    }
}

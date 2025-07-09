using SGHR.Application.Dtos;
using SGHR.Domain.Base;

namespace SGHR.Application.Contracts.Service
{
    public interface IHistorialReservaService
    {
        Task<OperationResult<IEnumerable<HistorialReservaDto>>> ObtenerHistorialAsync(
            int clienteId,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            string estado = null,
            string tipoHabitacion = null
        );

        Task<OperationResult<HistorialReservaDto>> ObtenerDetalleAsync(int idReserva, int clienteId);
    }
}

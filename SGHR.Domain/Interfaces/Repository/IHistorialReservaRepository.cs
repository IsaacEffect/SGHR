using SGHR.Domain.Entities.Historial;

namespace SGHR.Domain.Interfaces.Repository
{
    public interface IHistorialReservaRepository
    {
        Task<IEnumerable<HistorialReserva>> GetHistorialByClienteAsync(
            int clienteId,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            string estado = null,
            string tipoHabitacion = null
        );

        Task<HistorialReserva> GetDetalleReservaAsync(int idReserva, int clienteId);
    }
}

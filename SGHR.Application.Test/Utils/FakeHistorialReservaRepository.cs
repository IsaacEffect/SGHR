using SGHR.Domain.Entities.Historial;
using SGHR.Domain.Interfaces.Repository;

namespace SGHR.Application.Test.Utils
{
    public class FakeHistorialReservaRepository : IHistorialReservaRepository
    {
        private readonly List<HistorialReserva> _reservas = new();

        public void AgregarReserva(HistorialReserva reserva) => _reservas.Add(reserva);

        public Task<IEnumerable<HistorialReserva>> GetHistorialByClienteAsync(int clienteId, DateTime? fechaInicio = null, DateTime? fechaFin = null, string estado = null, string tipoHabitacion = null)
        {
            var resultado = _reservas.Where(r =>
                r.ClienteId == clienteId &&
                (fechaInicio == null || r.FechaEntrada >= fechaInicio) &&
                (fechaFin == null || r.FechaSalida <= fechaFin) &&
                (estado == null || r.Estado == estado) &&
                (tipoHabitacion == null || r.TipoHabitacion == tipoHabitacion));

            return Task.FromResult(resultado.AsEnumerable());
        }

        public Task<HistorialReserva> GetDetalleReservaAsync(int idReserva, int clienteId)
        {
            var resultado = _reservas.FirstOrDefault(r => r.Id == idReserva && r.ClienteId == clienteId);
            return Task.FromResult(resultado);
        }
    }
}

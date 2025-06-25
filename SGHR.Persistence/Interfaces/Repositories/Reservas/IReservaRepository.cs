using SGHR.Domain.Entities.Reservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Interfaces.Repositories.Reservas
{
    public interface IReservaRepository
    {
        Task<Reserva?> ObtenerPorIdAsync(int IdReserva);
        Task<List<Reserva>> ObtenerPorClienteIdAsync(int IdCliente);
        Task<List<Reserva>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta);
        Task<bool> HayDisponibilidadAsync(int habitacionId, DateTime fechaEntrada, DateTime fechaSalida);
        Task CrearAsync(Reserva reserva);
        Task ActualizarAsync(Reserva reserva);
        Task EliminarAsync(int IdReserva);
    }
}

using SGHR.Domain.Entities.Reservas;
using SGHR.Persistence.Interfaces.Repositories.Base;
namespace SGHR.Persistence.Interfaces.Repositories.Reservas
{
    public interface IReservaRepository : IBaseRepository<Reserva>
    {
        Task<List<Reserva>> ObtenerPorClienteIdAsync(int idCliente);
        Task<List<Reserva>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta);
        Task<List<Reserva>> ObtenerTodasAsync(bool IncluirRelaciones = false);
        Task<bool> HayDisponibilidadAsync(int habitacionId, DateTime fechaEntrada, DateTime fechaSalida, int? idReservaActual = null);
        Task CrearReservaAsync(Reserva reserva); 
        Task ActualizarReservaAsync(Reserva reserva);         
        Task CancelarReservaAsync(int idReserva);
    }
}

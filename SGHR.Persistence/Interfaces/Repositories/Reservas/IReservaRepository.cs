using SGHR.Domain.Entities.Reservas;
using SGHR.Persistence.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Interfaces.Repositories.Reservas
{
    public interface IReservaRepository : IBaseRepository<Reserva>
    {
        Task<Reserva?> ObtenerPorIdAsync(int idReserva);
        Task<List<Reserva>> ObtenerPorClienteIdAsync(int idCliente);
        Task<List<Reserva>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta);

        
        Task<bool> HayDisponibilidadAsync(int habitacionId, DateTime fechaEntrada, DateTime fechaSalida);

       
        Task CrearAsync(Reserva reserva); 

        
        Task ActualizarAsync(Reserva reserva); 

        
        Task CancelarReservaAsync(int idReserva);
    }
}

using SGHR.Domain.Entities.Historial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Interfaces
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

using SGHR.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Interfaces.Service
{
    public interface IHistorialReservaService
    {
        Task<IEnumerable<HistorialReservaDto>> ObtenerHistorialAsync(
            int clienteId,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            string estado = null,
            string tipoHabitacion = null
        );

        Task<HistorialReservaDto> ObtenerDetalleAsync(int idReserva, int clienteId);
    }
}

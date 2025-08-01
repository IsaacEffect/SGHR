using SGHR.Web.Models;

namespace SGHR.Web.Service.Contracts
{
    public interface IApiHistorialService
    {
        Task<List<HistorialModel>?> FiltrarHistorialAsync(int clienteId, DateTime? fechaInicio, DateTime? fechaFin, string estado, string tipoHabitacion);
        Task<HistorialModel?> ObtenerDetalleReservaAsync(int idReserva, int idCliente);
    }
}

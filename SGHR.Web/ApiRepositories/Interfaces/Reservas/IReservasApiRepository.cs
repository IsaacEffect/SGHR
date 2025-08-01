using SGHR.Web.Models;
using SGHR.Web.ViewModel.Reservas;

namespace SGHR.Web.ApiRepositories.Interfaces.Reservas
{
    public interface IReservasApiRepository
    {
        Task<ApiResponse<ReservasViewModel>> CrearReservaAsync(CrearReservaViewModel request);
        Task<ApiResponse<bool>> ActualizarReservaAsync(int id, ActualizarReservaViewModel request);
        Task<ApiResponse<bool>> CancelarReservaAsync(CancelarReservaViewModel request);
        Task<ApiResponse<ReservasViewModel>> ObtenerReservaPorIdAsync(int id);
        Task<ApiResponse<List<ReservasViewModel>>> ObtenerReservasPorClienteIdAsync(int clienteId);
        Task<ApiResponse<List<ReservasViewModel>>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta);
        Task<ApiResponse<List<ReservasViewModel>>> ObtenerTodasReservasAsync(bool incluirRelaciones = false);
    }
}
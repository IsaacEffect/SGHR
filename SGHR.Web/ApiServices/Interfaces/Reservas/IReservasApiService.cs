using SGHR.Web.ViewModel.Reservas;
using SGHR.Web.Models;
using SGHR.Application.DTOs.Reservas;
namespace SGHR.Web.ApiServices.Interfaces.Reservas
{
    public interface IReservasApiService
    {
        Task<ApiResponse<ReservasViewModel>> CrearReservaAsync(CrearReservaViewModel request);
        Task<ApiResponse<bool>> ActualizarReservaAsync(int id, ActualizarReservaViewModel request);
        Task<ApiResponse<bool>> CancelarReservaAsync(CancelarReservaViewModel request);
        Task<ApiResponse<ActualizarReservaViewModel>> ObtenerReservaPorIdAsync(int id);
        Task<ApiResponse<List<ReservasViewModel>>> ObtenerReservasPorClienteIdAsync(int clienteId);
        Task<ApiResponse<List<ReservasViewModel>>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta);
        Task<ApiResponse<List<ReservasViewModel>>> ObtenerTodasReservasAsync(bool incluirRelaciones = false);
    }
}

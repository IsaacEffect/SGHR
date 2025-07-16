using SGHR.Application.DTOs.Reservas;

namespace SGHR.Application.Interfaces.Reservas
{
    public interface IReservaApplicationService
    {
        Task<ReservaDto?> ObtenerReservaPorIdAsync(int id);
        Task<List<ReservaDto>> ObtenerReservasPorClienteIdAsync(int idCliente);
        Task<List<ReservaDto>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta);
        Task<List<ReservaDto>> ObtenerTodasReservasAsync();
        Task<ReservaDto> CrearReservaAsync(CrearReservaRequest request);
        Task<bool> ActualizarReservaAsync(int id, ActualizarReservaRequest request);
        Task<bool> CancelarReservaAsync(int id);
        Task<bool> VerificarDisponibilidadAsync(VerificarDisponibilidadRequest request);
    }
}

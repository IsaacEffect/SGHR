using SGHR.Application.DTOs.Reservas;

namespace SGHR.Application.Interfaces.Reservas
{
    public interface IReservaApplicationService
    {
        Task<ReservaDto> CrearReservaAsync(CrearReservaRequest request);
        Task<ReservaDto?> ObtenerReservaPorIdAsync(int idReserva);
        Task<List<ReservaDto>> ObtenerReservasPorClienteIdAsync(int idCliente);
        Task<List<ReservaDto>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta);
        Task CancelaReservaAsync (int idReserva);
        Task ActualizarReservaAsync(ActualizarReservaRequest request);
        Task<DisponibilidadDto> VerificarDisponibilidadAsync(VerificarDisponibilidadRequest request);
    }
}

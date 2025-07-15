using SGHR.Domain.Enums;
using SGHR.Application.DTOs.Reservas;
using SGHR.Domain.Entities.Reservas;
namespace SGHR.Application.Interfaces.Reservas
{
    public interface IReservaRules
    {
        Task ValidarExistenciaClienteAsync(int clienteId);
        Task ValidarExistenciaCategoriaAsync(int categoriaId, bool estaDisponible);
        Task ValidarReservaExistenteAsync(int idReserva);
        Task ValidarFechaEntradaMayorSalida(DateTime entrada, DateTime salida);
        Task ValidarTransicionEstadoAsync(EstadoReserva actual, EstadoReserva nuevo);
        Task<bool> RequiereVerificarDisponibilidad(Reserva reserva, ActualizarReservaRequest request);
        Task AplicarCambiosDeEstado(Reserva reserva, EstadoReserva nuevoEstado);
    }
}

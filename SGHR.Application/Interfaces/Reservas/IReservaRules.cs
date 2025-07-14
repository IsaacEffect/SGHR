using SGHR.Domain.Enums;
namespace SGHR.Application.Interfaces.Reservas
{
    public interface IReservaRules
    {
        Task ValidarExistenciaClienteAsync(int clienteId);
        Task ValidarExistenciaCategoriaAsync(int categoriaId, bool estaDisponible);
        Task ValidarReservaExistenteAsync(int idReserva);
        Task ValidarFechaEntradaMayorSalida(DateTime entrada, DateTime salida);
        Task ValidarTransicionEstadoAsync(EstadoReserva actual, EstadoReserva nuevo);
    }
}

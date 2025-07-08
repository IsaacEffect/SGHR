using SGHR.Domain.Enums;
namespace SGHR.Application.Interfaces.Reservas
{
    public interface IReservaRules
    {
        Task ValidarExistenciaClienteAsync(int clienteId);
        Task ValidarExistenciaCategoriaAsync(int categoriaId);
        Task ValidarReservaExistenteAsync(int idReserva);
        Task ValidarRangoFechasAsync(DateTime desde, DateTime hasta);
        Task ValidarFechaEntradaMayorSalida(DateTime entrada, DateTime salida);
        Task ValidarTransicionEstadoAsync(EstadoReserva actual, EstadoReserva nuevo);
    }
}

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
        Task ValidarMotivoCancelacion(string motivoCancelacion);
    }
}

using Microsoft.IdentityModel.Tokens;
using SGHR.Application.DTOs.Reservas;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Reservas;
using SGHR.Domain.Enums;
using SGHR.Persistence.Interfaces.Repositories.Clientes;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Reservas;

namespace SGHR.Application.Validations
{
    public class ReservaRules(
        IReservaRepository reservaRepository,
        ICategoriaHabitacionRepository categoriaHabitacionRepository,
        IClienteRepository clienteRepository) : IReservaRules
    {
        private readonly IReservaRepository _reservaRepository = reservaRepository;
        private readonly ICategoriaHabitacionRepository _categoriaHabitacionRepository = categoriaHabitacionRepository;
        private readonly IClienteRepository _clienteRepository = clienteRepository;

        public async Task ValidarExistenciaClienteAsync(int clienteId)
        {
            _ = await _clienteRepository.ObtenerPorId(clienteId)
                ?? throw new ArgumentException($"El cliente con el ID {clienteId} no existe.");
        }
        public async Task ValidarExistenciaCategoriaAsync(int categoriaId, bool estaDisponible)
        {

            if (!estaDisponible)
                _ = await _categoriaHabitacionRepository.ObtenerPorId(categoriaId)
                    ?? throw new ArgumentException($"La categoria con ID {categoriaId} no existe.");
        }
        public async Task ValidarReservaExistenteAsync(int idReserva)
        {
            _ = await _reservaRepository.ObtenerPorId(idReserva)
                ?? throw new InvalidOperationException($"No se encontró una reserva con el ID {idReserva}.");

        }

        // MOVER
        public Task ValidarMotivoCancelacion(string motivoCancelacion)
        {
            if (motivoCancelacion.IsNullOrEmpty())
            {
                throw new InvalidOperationException("El motivo no puede ir vacio");
            }
            else
            {
                return Task.CompletedTask;
            }
        }
    }
}

using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Clientes;
using SGHR.Persistence.Interfaces.Repositories.Reservas;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Enums;
using Azure.Core;

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
 
        public Task ValidarFechaEntradaMayorSalida(DateTime entrada, DateTime salida)
        {
            if (entrada > salida)
            {
                throw new ArgumentException("La fecha de entrada no puede ser posterior a la fecha de salida.");
            }
            return Task.CompletedTask;
        }

        public Task ValidarTransicionEstadoAsync(EstadoReserva actual, EstadoReserva nuevo)
        {
            if (nuevo == EstadoReserva.Confirmada && actual == EstadoReserva.Pendiente) return Task.CompletedTask; 
            if (nuevo == EstadoReserva.Finalizada && actual == EstadoReserva.Confirmada) return Task.CompletedTask; 
            if (nuevo == EstadoReserva.Cancelada && actual == EstadoReserva.Confirmada) return Task.CompletedTask;
            if (nuevo != actual)
                throw new InvalidOperationException($"El estado de la reserva no puede ser cambiado a {nuevo} desde el estado actual {actual}.");

            return Task.CompletedTask;
        }
    }
}

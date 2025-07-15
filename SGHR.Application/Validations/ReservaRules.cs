using Azure.Core;
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


        public Task VerificarReservaFinalizada(EstadoReserva estado)
        {
            if(estado == EstadoReserva.Finalizada)
            {
                throw new InvalidOperationException("No se puede cancelar una reserva que ya ha sido finalizada");
            }

           return Task.CompletedTask;
        }

        public Task<bool> RequiereVerificarDisponibilidad(Reserva reserva, ActualizarReservaRequest request)
        {
            bool fechaCambio = reserva.FechaEntrada != request.FechaEntrada || reserva.FechaSalida != request.FechaSalida;
            bool habitacionCambio = reserva.IdCategoriaHabitacion != request.IdCategoriaHabitacion;

            return Task.FromResult(fechaCambio || habitacionCambio);
        }

        public Task AplicarCambiosDeEstado(Reserva reserva, EstadoReserva nuevoEstado)
        {
            switch (nuevoEstado)
            {
                case EstadoReserva.Confirmada:
                    reserva.Activar();
                    break;
                case EstadoReserva.Cancelada:
                    reserva.Cancelar();
                    break;
                case EstadoReserva.Finalizada:
                    reserva.Finalizar();
                    break;
                default:
                    throw new InvalidOperationException($"El estado {nuevoEstado} no es válido para aplicar cambios.");


            }
            return Task.CompletedTask;
        }
    }
}

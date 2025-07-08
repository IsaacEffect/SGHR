using AutoMapper;
using SGHR.Application.DTOs.Reservas;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Reservas; 
using SGHR.Domain.Enums;
using SGHR.Domain.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Clientes;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones; 
using SGHR.Persistence.Interfaces.Repositories.Reservas;

namespace SGHR.Application.Services.Reservas
{
    public class ReservaApplicationService(
        IReservaRepository reservaRepository,
        ICategoriaHabitacionRepository categoriaHabitacionRepository,
        IClienteRepository clienteRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IReservaRules reservaRules) : IReservaApplicationService
    {
        private readonly IReservaRepository _reservaRepository = reservaRepository;
        private readonly ICategoriaHabitacionRepository _categoriaHabitacionRepository = categoriaHabitacionRepository; 
        private readonly IClienteRepository _clienteRepository = clienteRepository; 
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IReservaRules _reservaRules = reservaRules;
        public async Task<ReservaDto?> ObtenerReservaPorIdAsync(int id)
        {
            await _reservaRules.ValidarReservaExistenteAsync(id);
            var reserva = await _reservaRepository.ObtenerPorId(id);
            var dto = _mapper.Map<ReservaDto?>(reserva);
            return dto;
        }
        public async Task<List<ReservaDto>> ObtenerReservasPorClienteIdAsync(int idCliente)
        {
            await _reservaRules.ValidarExistenciaClienteAsync(idCliente);
            var reservas = await _reservaRepository.ObtenerPorClienteIdAsync(idCliente);
            var dto = _mapper.Map<List<ReservaDto>>(reservas);
            return dto;
        }
        public async Task<List<ReservaDto>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta)
        {
            await _reservaRules.ValidarRangoFechasAsync(desde, hasta);
            var reservas = await _reservaRepository.ObtenerReservasEnRangoAsync(desde, hasta);
            var dto = _mapper.Map<List<ReservaDto>>(reservas);
            return dto;
        }
        public async Task<ReservaDto> CrearReservaAsync(CrearReservaRequest request)
        {

            await _reservaRules.ValidarFechaEntradaMayorSalida(request.FechaEntrada, request.FechaSalida);
            try {
                
                await _reservaRules.ValidarExistenciaClienteAsync(request.ClienteId);
                await _reservaRules.ValidarExistenciaCategoriaAsync(request.IdCategoriaHabitacion);
                var nuevaReserva = _mapper.Map<Reserva>(request);
                nuevaReserva.GenerarNumeroReservaUnico();
                nuevaReserva.SetFechaUltimaModificacion();

                nuevaReserva.Activar();

                
                await _reservaRepository.CrearAsync(nuevaReserva);
                await _unitOfWork.CommitAsync();
                var dto = _mapper.Map<ReservaDto>(nuevaReserva);

                return dto;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Error de validación al crear la reserva: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Error al crear la reserva: {ex.Message}");
            }
           
        }
        public async Task<bool> ActualizarReservaAsync(int id, ActualizarReservaRequest request)
        {
            await _reservaRules.ValidarReservaExistenteAsync(id); 

            try
            {
                var reservaExistente = await _reservaRepository.ObtenerPorId(id)
                    ?? throw new KeyNotFoundException($"Reserva con ID {id} no encontrada.");

                await _reservaRules.ValidarFechaEntradaMayorSalida(request.FechaEntrada, request.FechaSalida);
                await _reservaRules.ValidarExistenciaClienteAsync(request.IdCliente);
                await _reservaRules.ValidarExistenciaCategoriaAsync(request.IdCategoriaHabitacion);

                bool fechaCambio =
                    reservaExistente.FechaEntrada != request.FechaEntrada ||
                    reservaExistente.FechaSalida != request.FechaSalida;
                bool habitacionCambio =
                    reservaExistente.IdCategoriaHabitacion != request.IdCategoriaHabitacion;
                bool necesitaVerificarDisponibilidad = fechaCambio || habitacionCambio;

                if (necesitaVerificarDisponibilidad)
                {
                    var estaDisponible = await _reservaRepository.HayDisponibilidadAsync(
                        request.IdCategoriaHabitacion,
                        request.FechaEntrada,
                        request.FechaSalida,
                        id
                    );
                    if (!estaDisponible)
                    {
                        throw new InvalidOperationException($"No hay disponibilidad para la categoría de habitación {request.IdCategoriaHabitacion} en el rango de fechas especificado para la actualización.");
                    }
                }

                reservaExistente.ActualizarDetalles(
                    request.IdCliente,
                    request.IdCategoriaHabitacion,
                    request.FechaEntrada,
                    request.FechaSalida,
                    request.NumeroHuespedes
                );

                await _reservaRules.ValidarTransicionEstadoAsync(reservaExistente.Estado, request.Estado);
                
                if (request.Estado == EstadoReserva.Confirmada)
                    reservaExistente.Confirmar();
                else if (request.Estado == EstadoReserva.Finalizada)
                    reservaExistente.Finalizar();
                else if (request.Estado == EstadoReserva.Cancelada)
                    reservaExistente.Cancelar();

                await _reservaRepository.ActualizarAsync(reservaExistente);
                await _unitOfWork.CommitAsync();

                return true;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Reserva con ID {id} no encontrada: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Error al actualizar la reserva: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Error de validación al actualizar la reserva: {ex.Message}");
            }
        }
        public async Task<bool> CancelarReservaAsync(int id)
        {
            try
            {
                var reservaExistente = await _reservaRepository.ObtenerPorId(id)
                   ?? throw new KeyNotFoundException($"Reserva con ID {id} no encontrada.");

                await _reservaRepository.CancelarReservaAsync(id);
                return true;
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidOperationException($"Error al cancelar la reserva: {ex.Message}");
            }
        }
        public async Task<bool> VerificarDisponibilidadAsync(VerificarDisponibilidadRequest request)
        {
            await _reservaRules.ValidarFechaEntradaMayorSalida(request.FechaEntrada, request.FechaSalida);
            await _reservaRules.ValidarExistenciaCategoriaAsync(request.IdCategoriaHabitacion);
            return await _reservaRepository.HayDisponibilidadAsync(
                request.IdCategoriaHabitacion,
                request.FechaEntrada,
                request.FechaSalida
            );

        }
    }
}
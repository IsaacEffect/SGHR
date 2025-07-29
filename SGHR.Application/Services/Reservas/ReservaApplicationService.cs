using AutoMapper;
using SGHR.Application.DTOs.Reservas;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Reservas; 
using SGHR.Domain.Enums;
using SGHR.Domain.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones; 
using SGHR.Persistence.Interfaces.Repositories.Reservas;

namespace SGHR.Application.Services.Reservas
{

    public class ReservaApplicationService(
        IReservaRepository reservaRepository,
        ICategoriaHabitacionRepository categoriaHabitacionRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IReservaRules reservaRules ) : IReservaApplicationService
    {
        private readonly IReservaRepository _reservaRepository = reservaRepository;
        private readonly ICategoriaHabitacionRepository _categoriaHabitacionRepository = categoriaHabitacionRepository;
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
        public async Task<List<ReservaDto>> ObtenerTodasReservasAsync(bool incluirRelaciones = false)
        {
            var reservas = await _reservaRepository.ObtenerTodasAsync(incluirRelaciones);
            var dto = _mapper.Map<List<ReservaDto>>(reservas);
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
            _ = new RangoFechas(desde, hasta);
            var reservas = await _reservaRepository.ObtenerReservasEnRangoAsync(desde, hasta);
            var dto = _mapper.Map<List<ReservaDto>>(reservas);
            return dto;
        }
        public async Task<ReservaDto> CrearReservaAsync(CrearReservaRequest request)
        {
            var estaDisponible = await _categoriaHabitacionRepository.HayDisponibilidadAsync
               (
                   request.IdCategoriaHabitacion,
                   request.FechaEntrada,
                   request.FechaSalida,
                   null
               );

            try
            {

                await _reservaRules.ValidarExistenciaClienteAsync(request.ClienteId);
                await _reservaRules.ValidarExistenciaCategoriaAsync(request.IdCategoriaHabitacion, estaDisponible);
                var nuevaReserva = _mapper.Map<Reserva>(request);
                _ = new RangoFechas(request.FechaEntrada, request.FechaSalida);
                nuevaReserva.GenerarNumeroReservaUnico();
                nuevaReserva.SetFechaUltimaModificacion();
                nuevaReserva.Activar();
                await _reservaRepository.CrearReservaAsync(nuevaReserva);
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
            var reservaExistente = await _reservaRepository.ObtenerPorId(id)
                    ?? throw new KeyNotFoundException($"Reserva con ID {id} no encontrada.");
            await _reservaRules.ValidarExistenciaClienteAsync(request.IdCliente);
            if (reservaExistente.RequiereVerificarDisponibilidad(request.FechaEntrada, request.FechaSalida, request.IdCategoriaHabitacion))
            {
                var estaDisponible = await _categoriaHabitacionRepository.HayDisponibilidadAsync
                    (
                        request.IdCategoriaHabitacion,
                        request.FechaEntrada,
                        request.FechaSalida,
                        id
                     );
                await _reservaRules.ValidarExistenciaCategoriaAsync(request.IdCategoriaHabitacion, estaDisponible);
            }
            reservaExistente.ActualizarDetalles(
                request.IdCliente,
                request.IdCategoriaHabitacion,
                request.FechaEntrada,
                request.FechaSalida,
                request.NumeroHuespedes,
                request.Estado
            );
            await _reservaRepository.ActualizarReservaAsync(reservaExistente);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> CancelarReservaAsync(int id, CancelarReservaDto request)
        {
            var reservaExistente = await _reservaRepository.ObtenerPorId(id)
                  ?? throw new KeyNotFoundException($"Reserva con ID {id} no encontrada.");

            if (reservaExistente.Estado == EstadoReserva.Finalizada)
                throw new InvalidOperationException("No se puede cancelar una reserva que ya esta finalizada.");
            try
            { 
                await _reservaRules.ValidarReservaExistenteAsync(id);
                await _reservaRules.ValidarMotivoCancelacion(request.MotivoCancelacion);

                await _reservaRepository.CancelarReservaAsync(id, request.MotivoCancelacion);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidOperationException($"Error al cancelar la reserva: {ex.Message}");
            }
        }
    }
}
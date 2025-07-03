using AutoMapper;
using SGHR.Application.DTOs.Reservas;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Reservas; 
using SGHR.Domain.enums; 
using SGHR.Persistence.Context; 
using SGHR.Persistence.Interfaces.Repositories.Clientes;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones; 
using SGHR.Persistence.Interfaces.Repositories.Reservas;
namespace SGHR.Application.Services.Reservas
{
    public class ReservaApplicationService : IReservaApplicationService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly ICategoriaHabitacionRepository _categoriaHabitacionRepository; 
        private readonly IClienteRepository _clienteRepository; 
        private readonly IMapper _mapper;
        private readonly SGHRDbContext _dbContext;
        public ReservaApplicationService(
            IReservaRepository reservaRepository,
            ICategoriaHabitacionRepository categoriaHabitacionRepository, 
            IClienteRepository clienteRepository, 
            IMapper mapper,
            SGHRDbContext dbContext)
        {
            _reservaRepository = reservaRepository;
            _categoriaHabitacionRepository = categoriaHabitacionRepository;
            _clienteRepository = clienteRepository;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<ReservaDto?> ObtenerReservaPorIdAsync(int id)
        {
            var reserva = await _reservaRepository.ObtenerPorId(id);
            var dto = _mapper.Map<ReservaDto?>(reserva);
            return dto;
        }

        public async Task<List<ReservaDto>> ObtenerReservasPorClienteIdAsync(int idCliente)
        {
            var clienteExiste = await _clienteRepository.ObtenerPorId(idCliente) 
                                ?? throw new ArgumentException($"El cliente con ID {idCliente} no existe.");
            var reservas = await _reservaRepository.ObtenerPorClienteIdAsync(idCliente);
            var dto = _mapper.Map<List<ReservaDto>>(reservas);
            return dto;
        }
        public async Task<List<ReservaDto>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta)
        {
            if(desde >= hasta)
            {
                throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin.");
            }
            var reservas = await _reservaRepository.ObtenerReservasEnRangoAsync(desde, hasta);
            var dto = _mapper.Map<List<ReservaDto>>(reservas);
            return dto;
        }

        public async Task<ReservaDto> CrearReservaAsync(CrearReservaRequest request)
        {

            Console.WriteLine($"Creando reserva para cliente ID: {request.ClienteId}, categoría de habitación ID: {request.IdCategoriaHabitacion}, fechas: {request.FechaEntrada} a {request.FechaSalida}");
            if (request.FechaEntrada >= request.FechaSalida)
            {
                throw new ArgumentException("La fecha de entrada debe ser anterior a la fecha de salida.");
            }
            var clienteExistente = await _clienteRepository.ObtenerPorId(request.ClienteId);
            try {
                
                if (clienteExistente is null)
                {
                    throw new ArgumentException($"El cliente con ID {request.ClienteId} no existe.");
                }
                var habitacionExistente = await _categoriaHabitacionRepository.ObtenerPorIdAsync(request.IdCategoriaHabitacion) 
                                          ?? throw new ArgumentException($"La categoría de habitación con ID {request.IdCategoriaHabitacion} no existe.");
                var nuevaReserva = _mapper.Map<Reserva>(request);
                nuevaReserva.GenerarNumeroReservaUnico();
                nuevaReserva.SetFechaUltimaModificacion();

                nuevaReserva.Activar();

                
                await _reservaRepository.CrearAsync(nuevaReserva);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine($"ID generado por EF: {nuevaReserva.Id}");
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
            var reservaExistente = await _reservaRepository.ObtenerPorId(id);

            try
            {
                if (reservaExistente == null)
                {
                    return false;
                }

                if (request.FechaEntrada >= request.FechaSalida)
                {
                    throw new ArgumentException("La fecha de entrada debe ser anterior a la fecha de salida.");
                }

                if (reservaExistente.ClienteId != request.IdCliente)
                {
                    var clienteExiste = await _clienteRepository.ObtenerPorId(request.IdCliente) ?? throw new KeyNotFoundException($"Nuevo Cliente con ID {request.IdCliente} no encontrado.");
                }
                if (reservaExistente.IdCategoriaHabitacion != request.IdCategoriaHabitacion)
                {
                    var categoriaExiste = await _categoriaHabitacionRepository.ObtenerPorIdAsync(request.IdCategoriaHabitacion) != null;
                    if (!categoriaExiste) throw new KeyNotFoundException($"Nueva Categoría de habitación con ID {request.IdCategoriaHabitacion} no encontrada.");
                }

                bool necesitaVerificarDisponibilidad =
                    reservaExistente.FechaEntrada != request.FechaEntrada ||
                    reservaExistente.FechaSalida != request.FechaSalida ||
                    reservaExistente.IdCategoriaHabitacion != request.IdCategoriaHabitacion;

                if (necesitaVerificarDisponibilidad)
                {
                    var estaDisponible = await _reservaRepository.HayDisponibilidadAsync(
                        request.IdCategoriaHabitacion,
                        request.FechaEntrada,
                        request.FechaSalida
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

                if (request.Estado == EstadoReserva.Confirmada && reservaExistente.Estado == EstadoReserva.Pendiente)
                {
                    reservaExistente.Confirmar();
                }
                else if (request.Estado == EstadoReserva.Finalizada && reservaExistente.Estado == EstadoReserva.Confirmada)
                {
                    reservaExistente.Finalizar();
                }
                else if (request.Estado == EstadoReserva.Cancelada && reservaExistente.Estado == EstadoReserva.Confirmada)
                {
                    reservaExistente.Cancelar();
                }
                else if (request.Estado != reservaExistente.Estado)
                {
                    throw new InvalidOperationException($"El estado de la reserva no puede ser cambiado a {request.Estado} desde el estado actual {reservaExistente.Estado}.");
                }

                await _reservaRepository.ActualizarAsync(reservaExistente);
                await _dbContext.SaveChangesAsync();

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
            catch(ArgumentException ex)
            {
                throw new ArgumentException($"Error de validacion al actualizar la reserva: {ex.Message}");
            }
        }

        public async Task<bool> CancelarReservaAsync(int id)
        {
            var reservaExistente = await _reservaRepository.ObtenerPorId(id)
                                   ?? throw new KeyNotFoundException($"Reserva con ID {id} no encontrada.");
            try
            {
                reservaExistente.Cancelar();
                await _reservaRepository.ActualizarAsync(reservaExistente);
                return true;
            }
            catch(InvalidCastException ex)
            {
                throw new InvalidOperationException($"Error al cancelar la reserva: {ex.Message}");
            }
        }

        public async Task<bool> VerificarDisponibilidadAsync(VerificarDisponibilidadRequest request)
        {
            if(request.FechaEntrada >= request.FechaSalida)
            {
                throw new ArgumentException("La fecha de entrada debe ser anterior a la fecha de salida.");
            }
            var categoriaExiste = await _categoriaHabitacionRepository.ObtenerPorIdAsync(request.IdCategoriaHabitacion);
            return categoriaExiste is null
                ? throw new ArgumentException($"La categoría de habitación con ID {request.IdCategoriaHabitacion} no existe.")
                : await _reservaRepository.HayDisponibilidadAsync(
                request.IdCategoriaHabitacion,
                request.FechaEntrada,
                request.FechaSalida
            );
        }
    }
}

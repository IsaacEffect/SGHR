using SGHR.Application.DTOs.Reservas;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Reservas;
using SGHR.Persistence.Interfaces.Repositories.Reservas;

namespace SGHR.Application.Services.Reservas
{
    public class ReservaApplicationService : IReservaApplicationService
    {
        readonly IReservaApplicationService _reservaRepository;
        public ReservaApplicationService(IReservaApplicationService reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }

      
        public async Task<ReservaDto?> ObtenerReservaPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<ReservaDto>> ObtenerReservasPorClienteIdAsync(int idCliente)
        {
            throw new NotImplementedException();
        }
        public async Task<List<ReservaDto>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta)
        {
            throw new NotImplementedException();
        }

        public async Task<ReservaDto> CrearReservaAsync(CrearReservaRequest request)
        {
            throw new NotImplementedException("Implementar lógica de creación de reserva aquí");
        }
        public async Task CancelaReservaAsync(int idReserva)
        {
            throw new NotImplementedException("Implementar lógica de cancelación de reserva aquí");
        }
        public async Task ActualizarReservaAsync(ActualizarReservaRequest request)
        {
            throw new NotImplementedException("Implementar lógica de actualización de reserva aquí");
        }
        public async Task<DisponibilidadDto> VerificarDisponibilidadAsync(VerificarDisponibilidadRequest request)
        {
            throw new NotImplementedException("Implementar lógica de verificación de disponibilidad aquí");
        }
    }
}

using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Base;
using SGHR.Domain.Entities.Habitaciones;
using SGHR.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace SGHR.Persistence.Repositories.Habitaciones
{
    public class CategoriaHabitacionRepository : BaseRepository<CategoriaHabitacion>, ICategoriaHabitacionRepository
    {
        public CategoriaHabitacionRepository(SGHRDbContext context) : base(context)
        {

        }

        public async Task<CategoriaHabitacion> ObtenerPorIdAsync(int categoriaId)
        {
            return await base.ObtenerPorId(categoriaId)
                   ?? throw new KeyNotFoundException($"CategoriaHabitacion with ID {categoriaId} not found.");
        }
        public async Task<bool> HayDisponibilidadAsync(int categoriaId, DateTime fechaEntrada, DateTime fechaSalida, int? reservaId = null)
        {
            var reservas = await _context.Reservas
                .Where(r => r.IdCategoriaHabitacion == categoriaId &&
                            r.FechaEntrada < fechaSalida &&
                            r.FechaSalida > fechaEntrada &&
                            (reservaId == null || r.Id != reservaId))
                .ToListAsync();
            return !reservas.Any();
        }
    }
}

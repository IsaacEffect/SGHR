using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Base;
using SGHR.Domain.Entities.Habitaciones;
using SGHR.Persistence.Context;

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
    }
}

using Microsoft.EntityFrameworkCore;
using SGHR.Persistence.Base;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using ServiciosEntity = SGHR.Domain.Entities.Servicios.Servicios;

namespace SGHR.Persistence.Repositories.Servicios
{
    public class ServicioRepository(SGHRDbContext context) : BaseRepository<ServiciosEntity>(context), IServicioRepository
    {

        public async Task<ServiciosEntity?> ObtenerPorIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task AgregarServicioAsync(ServiciosEntity servicio)
        {
            ArgumentNullException.ThrowIfNull(servicio);
            await _dbSet.AddAsync(servicio);
        }
        public Task ActualizarServicioAsync(ServiciosEntity servicio)
        {
            ArgumentNullException.ThrowIfNull(servicio);
            _dbSet.Update(servicio);
            return Task.CompletedTask;
        }
        public async Task EliminarServicioAsync(int id)
        {
            var servicio = await base.ObtenerPorId(id) ?? throw new KeyNotFoundException($"Servicio con ID {id} no encontrado.");
            servicio.Eliminar();
            _dbSet.Update(servicio);
        }
        public Task<List<ServiciosEntity>> ObtenerServiciosActivosAsync()
        {
            return _dbSet
                .Where(s => s.Activo)
                .ToListAsync();
        }

        public Task<List<ServiciosEntity>> ObtenerTodosLosServiciosAsync()
        {
            return _dbSet.Where(s => !s.Eliminado).ToListAsync();    
        }
    }
}
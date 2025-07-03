using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGHR.Persistence.Base;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using ServiciosEntity = SGHR.Domain.Entities.Servicios.Servicios;

namespace SGHR.Persistence.Repositories.Servicios
{
    public class ServicioRepository(SGHRDbContext context, ISqlConnectionFactory sqlConnectionFactory) : BaseRepository<Domain.Entities.Servicios.Servicios>(context), IServicioRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<ServiciosEntity?> ObtenerPorIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task AgregarServicioAsync(ServiciosEntity servicio)
        {
            ArgumentNullException.ThrowIfNull(servicio);
            await _dbSet.AddAsync(servicio);
            await _context.SaveChangesAsync();
        }
        public async Task ActualizarServicioAsync(ServiciosEntity servicio)
        {
            ArgumentNullException.ThrowIfNull(servicio);
            _dbSet.Update(servicio);
            await _context.SaveChangesAsync();
        }
        public async Task EliminarServicioAsync(int id)
        {
            var servicio = await base.ObtenerPorId(id) ?? throw new KeyNotFoundException($"Servicio con ID {id} no encontrado.");
            _dbSet.Remove(servicio);
            await _context.SaveChangesAsync();
        }

        public Task ActivarServicioAsync(int id, bool activo)
        {
            return _context.Database.ExecuteSqlRawAsync(
                "EXEC ActivarServicio @IdServicio = {0}, @Estado = {1}",
                new SqlParameter("@IdServicio", id),
                new SqlParameter("@Estado", activo)
            );
        }

        public Task<List<ServiciosEntity>> ObtenerServiciosActivosAsync()
        {
            return _dbSet
                .Where(s => s.Activo)
                .ToListAsync();
        }

        public Task<List<ServiciosEntity>> ObtenerTodosLosServiciosAsync()
        {
            return _dbSet.ToListAsync();    
        }
    }
}
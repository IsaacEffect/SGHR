using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGHR.Domain.Entities.Servicios;
using SGHR.Persistence.Base;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.Servicios
{
    public class ServicioRepository : BaseRepository<Domain.Entities.Servicios.Servicios>, IServicioRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ServicioRepository(SGHRDbContext context, ISqlConnectionFactory sqlConnectionFactory) : base(context)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Domain.Entities.Servicios.Servicios?> ObtenerPorIdAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<List<Domain.Entities.Servicios.Servicios>> ObtenerTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AgregarServicioAsync(Domain.Entities.Servicios.Servicios servicio)
        {
 
            await AddAsync(servicio); ;    
        }
        public async Task ActualizarServicioAsync(Domain.Entities.Servicios.Servicios servicio)
        {
         
            await Task.Run(() =>
            {
                _context.Servicios.Update(servicio);
            });
        }

        public async Task EliminarServicioAsync(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
            }
        }
    }
}
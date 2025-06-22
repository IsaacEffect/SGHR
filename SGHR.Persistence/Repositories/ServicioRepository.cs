using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGHR.Domain.Entities.Servicios;
using SGHR.Persistence.Base;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces;
using SGHR.Persistence.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories
{
    public class ServicioRepository : BaseRepository<Servicio>, IServicioRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ServicioRepository(SGHRDbContext context, ISqlConnectionFactory sqlConnectionFactory) : base(context)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Servicio?> ObtenerPorIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<List<Servicio>> ObtenerTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AgregarServicioAsync(Servicio servicio)
        {
 
            await base.AddAsync(servicio); ;    
        }
        public async Task ActualizarServicioAsync(Servicio servicio)
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
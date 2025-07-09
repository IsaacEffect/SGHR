using Microsoft.EntityFrameworkCore;
using SGHR.Domain.InterfacesRepositories;
using SGHR.Persistance.DBContext;
using SGHR.Persistance.Models;
using SGHR.WebApp.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistance.Repositories
{
    public class HabitacionesRepository : IHabitacionesRepository
    {
        private readonly SGHRDbContext _context;
        private readonly DbSet<Habitaciones> _habitaciones;
        public HabitacionesRepository(SGHRDbContext context)
        {
            _context = context; ;
            _habitaciones = _context.Set<Habitaciones>();
        }

        

        public async Task<bool> ExistenPorCategoriaAsync(int idCategoria)
        {
            return await _context.Habitaciones
         .AnyAsync(h => h.IdCategoriaHabitacion == idCategoria);
        }
    }
}

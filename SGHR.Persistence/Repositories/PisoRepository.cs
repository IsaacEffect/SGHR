
using SGHR.Persistence.Context;
using SGHR.Persistence.Domain;
using SGHR.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories
{
    public class PisoRepository : IPisoRepository
    {
        private readonly SGHRContext _context;

        public PisoRepository(SGHRContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Piso>> GetAllAsync()
        {
            return await _context.Pisos.ToListAsync();
        }

        public async Task<Piso> GetByIdAsync(int id)
        {
            return await _context.Pisos.FindAsync(id);
        }

        public async Task AddAsync(Piso piso)
        {
            await _context.Pisos.AddAsync(piso);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Piso piso)
        {
            _context.Pisos.Update(piso);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var piso = await _context.Pisos.FindAsync(id);
            if (piso != null)
            {
                _context.Pisos.Remove(piso);
                await _context.SaveChangesAsync();
            }
        }
    }
}

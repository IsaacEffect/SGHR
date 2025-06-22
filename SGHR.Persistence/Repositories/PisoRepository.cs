using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SGHR.Persistence.Context;
using SGHR.Persistence.Domain;  // Cambié la referencia a Domain
using SGHR.Persistence.Interfaces;
using SGHR.Persistence.Base;

namespace SGHR.Persistence.Repositories
{
    public class PisoRepository : BaseRepository<Piso>, IPisoRepository
    {
        private readonly HotelContext _context;

        public PisoRepository(HotelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Piso>> GetAllPisosAsync()
        {
            return await _context.Pisos.ToListAsync();
        }

        public async Task<Piso> GetPisoByIdAsync(int id)
        {
            return await _context.Pisos.FindAsync(id);
        }

        public async Task CreatePisoAsync(Piso piso)
        {
            await base.CreateAsync(piso);
        }

        public async Task UpdatePisoAsync(Piso piso)
        {
            await base.UpdateAsync(piso);
        }

        public async Task DeletePisoAsync(int id)
        {
            var piso = await GetPisoByIdAsync(id);
            if (piso != null)
            {
                await base.DeleteAsync(piso);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Interfaces
{
    public interface IPisoRepository
    {
        Task<IEnumerable<Piso>> GetAllPisosAsync();
        Task<Piso> GetPisoByIdAsync(int id);
        Task CreatePisoAsync(Piso piso);
        Task UpdatePisoAsync(Piso piso);
        Task DeletePisoAsync(int id);
    }
}

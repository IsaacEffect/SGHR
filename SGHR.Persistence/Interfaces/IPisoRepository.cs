using SGHR.Persistence.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGHR.Persistence.Interfaces
{
    public interface IPisoRepository
    {
        Task<IEnumerable<Piso>> GetAllAsync();           
        Task<Piso> GetByIdAsync(int id);                 
        Task AddAsync(Piso piso);                       
        Task UpdateAsync(Piso piso);                     
        Task DeleteAsync(int id);                        
    }
}

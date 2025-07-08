using SGHR.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGHR.Application.Interfaces
{
    public interface IPisoService
    {
        Task<IEnumerable<PisoDto>> GetAllAsync();
        Task<PisoDto> GetByIdAsync(int id);
        Task AddAsync(PisoDto piso);
        Task UpdateAsync(PisoDto piso);
        Task DeleteAsync(int id);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using SGHR.Application.Dtos;   

namespace SGHR.Application.Interfaces
{
    public interface IPisoService
    {
        Task<IEnumerable<PisoDto>> GetAllAsync();
        Task<PisoDto?> GetByIdAsync(int id);
        Task AddAsync(SavePisoDto dto);
        Task UpdateAsync(UpdatePisoDto dto);
        Task DeleteAsync(int id);
    }
}

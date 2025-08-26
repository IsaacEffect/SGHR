using SGHR.Application.Dtos;

namespace SGHR.Application.Interfaces
{
    public interface IPisoRepository
    {
        Task<IEnumerable<PisoDto>> GetAllAsync();
        Task<PisoDto?> GetByIdAsync(int id);
        Task<PisoDto?> CreateAsync(SavePisoDto dto);
        Task<bool> UpdateAsync(UpdatePisoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

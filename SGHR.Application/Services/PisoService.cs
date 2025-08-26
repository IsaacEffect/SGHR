using SGHR.Application.Dtos;
using SGHR.Application.Dtos;
using SGHR.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGHR.Application.Services
{
    public class PisoService : IPisoService
    {
        private readonly IPisoRepository _pisoRepository;

        public PisoService(IPisoRepository pisoRepository)
        {
            _pisoRepository = pisoRepository;
        }

        // Obtener todos los pisos
        public async Task<IEnumerable<PisoDto>> GetAllAsync()
        {
            var pisos = await _pisoRepository.GetAllAsync();
            return pisos.Select(p => new PisoDto
            {
                Id = p.Id,
                NumeroPiso = p.NumeroPiso,
                Descripcion = p.Descripcion
            });
        }

        // Obtener un piso por id
        public async Task<PisoDto?> GetByIdAsync(int id)
        {
            var piso = await _pisoRepository.GetByIdAsync(id);
            if (piso == null) return null;

            return new PisoDto
            {
                Id = piso.Id,
                NumeroPiso = piso.NumeroPiso,
                Descripcion = piso.Descripcion
            };
        }

        // Crear un nuevo piso
        public async Task AddAsync(SavePisoDto dto)
        {
            var piso = new SavePisoDto
            {
                NumeroPiso = dto.NumeroPiso,
                Descripcion = dto.Descripcion
            };

            await _pisoRepository.CreateAsync(piso);
        }

        // Actualizar un piso existente
        public async Task UpdateAsync(UpdatePisoDto dto)
        {
            var piso = new UpdatePisoDto
            {
                Id = dto.Id,
                NumeroPiso = dto.NumeroPiso,
                Descripcion = dto.Descripcion
            };

            await _pisoRepository.UpdateAsync(piso);
        }

        // Eliminar un piso
        public async Task DeleteAsync(int id)
        {
            await _pisoRepository.DeleteAsync(id);
        }
    }
}

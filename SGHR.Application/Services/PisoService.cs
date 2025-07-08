using SGHR.Application.Dtos;
using SGHR.Application.Interfaces;
using SGHR.Persistence.Domain;
using SGHR.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGHR.Persistence.Domain; 


namespace SGHR.Application.Services
{
    public class PisoService : IPisoService
    {
        private readonly IPisoRepository _pisoRepository;

        public PisoService(IPisoRepository pisoRepository)
        {
            _pisoRepository = pisoRepository;
        }

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

        public async Task<PisoDto> GetByIdAsync(int id)
        {
            var p = await _pisoRepository.GetByIdAsync(id);
            if (p == null) return null;

            return new PisoDto
            {
                Id = p.Id,
                NumeroPiso = p.NumeroPiso,
                Descripcion = p.Descripcion
            };
        }

        public async Task AddAsync(PisoDto piso)
        {
            var entidad = new Piso
            {
                NumeroPiso = piso.NumeroPiso,
                Descripcion = piso.Descripcion
            };

            await _pisoRepository.AddAsync(entidad);
        }

        public async Task UpdateAsync(PisoDto piso)
        {
            var entidad = new Piso
            {
                Id = piso.Id,
                NumeroPiso = piso.NumeroPiso,
                Descripcion = piso.Descripcion
            };

            await _pisoRepository.UpdateAsync(entidad);
        }

        public async Task DeleteAsync(int id)
        {
            await _pisoRepository.DeleteAsync(id);
        }
    }
}

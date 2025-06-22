using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaGestionHotel.Application.Interfaces;
using SistemaGestionHotel.Domain.Entities;

namespace SistemaGestionHotel.Persistence.Repositories
{
    public class PisoRepository : IPisoRepository
    {
        private readonly List<Piso> _pisos = new();

        public async Task<IEnumerable<Piso>> ObtenerTodosAsync()
        {
            return await Task.FromResult(_pisos);
        }

        public async Task<Piso> ObtenerPorIdAsync(int id)
        {
            var piso = _pisos.FirstOrDefault(p => p.Id == id);
            return await Task.FromResult(piso);
        }

        public async Task AgregarAsync(Piso piso)
        {
            _pisos.Add(piso);
            await Task.CompletedTask;
        }

        public async Task ActualizarAsync(Piso piso)
        {
            var existente = _pisos.FirstOrDefault(p => p.Id == piso.Id);
            if (existente != null)
            {
                existente.Nombre = piso.Nombre;
                existente.Nivel = piso.Nivel;
            }
            await Task.CompletedTask;
        }

        public async Task EliminarAsync(int id)
        {
            var piso = _pisos.FirstOrDefault(p => p.Id == id);
            if (piso != null)
            {
                _pisos.Remove(piso);
            }
            await Task.CompletedTask;
        }
    }
}

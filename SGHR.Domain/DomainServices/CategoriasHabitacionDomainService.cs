using SGHR.Domain.InterfacesDomainServices;
using SGHR.Domain.InterfacesRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.DomainServices
{
    public sealed class CategoriasHabitacionDomainService : ICategoriasHabitacionesDomainService
    {
        private readonly IHabitacionesRepository _habitacionesrepository;
        public CategoriasHabitacionDomainService(IHabitacionesRepository habitacionesRepository) 
        { 
            _habitacionesrepository = habitacionesRepository;
        }

        public async Task NoEliminarCategoriasConHabitacionesAsignadas(int id)
        {
            bool existenHabitaciones = await _habitacionesrepository.ExistenPorCategoriaAsync(id);

            if (existenHabitaciones)
            {
                throw new Exception("No se puede eliminar: la categoría tiene habitaciones asignadas.");
            }
        }

       
    }
}

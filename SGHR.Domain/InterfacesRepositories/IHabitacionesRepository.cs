using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.InterfacesRepositories
{
    public interface IHabitacionesRepository
    {
       Task<bool> ExistenPorCategoriaAsync(int idCategoria);
        

    }
}

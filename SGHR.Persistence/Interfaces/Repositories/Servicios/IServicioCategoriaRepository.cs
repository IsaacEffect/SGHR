using SGHR.Domain.Entities.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Interfaces.Repositories.Servicios
{
    public interface IServicioCategoriaRepository
    {
        Task AgregarPrecioServicioCategoriaAsync(int servicioId, int categoriaId, decimal precio); 
        Task ActualizarPrecioServicioCategoriaAsync(int servicioId, int categoriaId, decimal precio); 
        Task EliminarPrecioServicioCategoriaAsync(int servicioId, int categoriaId);
    }
}

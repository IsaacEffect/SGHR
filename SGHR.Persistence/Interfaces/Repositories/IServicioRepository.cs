using SGHR.Domain.Entities.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Interfaces.Repositories
{
    public interface IServicioRepository
    {
       
        Task<Servicio?> ObtenerPorIdAsync(int id);
        Task<List<Servicio>> ObtenerTodosAsync();
        Task AgregarServicioAsync(Servicio servicio);
        Task ActualizarServicioAsync(Servicio servicio);
        Task EliminarServicioAsync(int id);
       

    }
}

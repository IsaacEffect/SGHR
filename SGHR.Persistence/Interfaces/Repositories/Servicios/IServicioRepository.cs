using SGHR.Domain.Entities.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Interfaces.Repositories.Servicios
{
    public interface IServicioRepository
    {
       
        Task<Domain.Entities.Servicios.Servicios?> ObtenerPorIdAsync(int id);
        Task<List<Domain.Entities.Servicios.Servicios>> ObtenerTodosAsync();
        Task AgregarServicioAsync(Domain.Entities.Servicios.Servicios servicio);
        Task ActualizarServicioAsync(Domain.Entities.Servicios.Servicios servicio);
        Task EliminarServicioAsync(int id);
       

    }
}

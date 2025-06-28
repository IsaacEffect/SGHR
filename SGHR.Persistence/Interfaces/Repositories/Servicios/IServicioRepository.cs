using System.Threading.Tasks;
using ServiciosEntity = SGHR.Domain.Entities.Servicios.Servicios;

namespace SGHR.Persistence.Interfaces.Repositories.Servicios
{
    public interface IServicioRepository
    {
        Task AgregarServicioAsync(ServiciosEntity servicio);
        Task ActualizarServicioAsync(ServiciosEntity servicio);
        Task EliminarServicioAsync(int id);
        Task<ServiciosEntity?> ObtenerPorIdAsync(int id);
        Task<List<ServiciosEntity>> ObtenerServiciosActivosAsync();
        Task<List<ServiciosEntity>> ObtenerTodosLosServiciosAsync();
        Task ActivarServicioAsync(int id, bool activo);

    }
}

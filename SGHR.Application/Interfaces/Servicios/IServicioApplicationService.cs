using SGHR.Application.DTOs.Servicios;

namespace SGHR.Application.Interfaces.Servicios
{
    public interface IServicioApplicationService
    {
        // Operaciones CRUD para servicios
        Task<ServicioDto> AgregarServicioAsync(AgregarServicioRequest request);
        Task ActualizarServicioAsync(ActualizarServicioRequest request);
        Task EliminarServicioAsync(int id);
        Task <List<ServicioDto>> ObtenerServiciosActivosAsync();
        Task<ServicioDto?> ObtenerServicioPorIdAsync(int id);
        Task<List<ServicioDto>> ObtenerTodosLosServiciosAsync();
        Task ActivarServicioAsync(int id);
        Task DesactivarServicioAsync(int id);

      
    }
}

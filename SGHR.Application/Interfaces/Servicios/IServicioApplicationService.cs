using SGHR.Application.DTOs.Servicios;

namespace SGHR.Application.Interfaces.Servicios
{
    public interface IServicioApplicationService
    {
        // Operaciones CRUD para servicios
        Task<ServicioDto> CrearServicioAsync(CrearServicioRequest request);
        Task ActualizarServicioAsync(ActualizarServicioRequest request);
        Task EliminarServicioAsync(int idServicio); 
        Task<ServicioDto?> ObtenerServicioPorIdAsync(int idServicio);
        Task<List<ServicioDto>> ObtenerTodosLosServiciosAsync();
        Task ActivarServicioAsync(int idServicio);
        Task DesactivarServicioAsync(int idServicio);

        // Operaciones para la asignacion de precios por categoria
        Task AsignarPrecioServicioCategoriaAsync(AsignarPrecioServicioCategoriaRequest request);
        Task EliminarPrecioServicioCategoriaAsync(int servicioId, int categoriaId);
        Task<List<ServicioCategoriaDto>> ObtenerPreciosServicioPorCategoriaAsync(int categoriaId);
        Task<List<ServicioCategoriaDto>> ObtenerPreciosCategoriaPorServicioAsync(int servicioId);
        Task<ServicioCategoriaDto?> ObtenerPrecioServicioCategoriaEspecificoAsync(int servicioId, int categoriaId);
    }
}

using SGHR.Application.DTOs.Servicios;

namespace SGHR.Application.Interfaces.Servicios
{
    public interface IServicioApplicationService
    {
        // Operaciones CRUD para servicios
        Task<ServicioDto> AgregarServicioAsync(AgregarServicioRequest request);
        Task ActualizarServicioAsync(ActualizarServicioRequest request); // <-- MODIFICADO
        Task EliminarServicioAsync(int id);
        Task<ServicioDto?> ObtenerServicioPorIdAsync(int id);
        Task<List<ServicioDto>> ObtenerTodosLosServiciosAsync();
        Task ActivarServicioAsync(int id);
        Task DesactivarServicioAsync(int id);

        // Operaciones para la asignacion de precios por categoria
        Task AsignarPrecioServicioCategoriaAsync(AsignarPrecioServicioCategoriaRequest request);
        Task EliminarPrecioServicioCategoriaAsync(int servicioId, int categoriaId);

        Task<List<ServicioCategoriaDto>> ObtenerPreciosServicioPorCategoriaAsync(int categoriaId);
        Task<List<ServicioCategoriaDto>> ObtenerPreciosCategoriaPorServicioAsync(int servicioId);
        Task<ServicioCategoriaDto?> ObtenerPrecioServicioCategoriaEspecificoAsync(int servicioId, int categoriaId);
    }
}

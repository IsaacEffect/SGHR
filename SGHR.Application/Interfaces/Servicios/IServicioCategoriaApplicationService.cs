using SGHR.Application.DTOs.Servicios;

namespace SGHR.Application.Interfaces.Servicios
{
    public interface IServicioCategoriaApplicationService
    {
        Task AsignarPrecioServicioCategoriaAsync(AsignarPrecioServicioCategoriaRequest request);
        Task ActualizarPrecioServicioCategoriaAsync(int servicioId, int categoriaId, decimal precio);
        Task EliminarPrecioServicioCategoriaAsync(int servicioId, int categoriaId);
        Task<List<ServicioCategoriaDto>> ObtenerPreciosServicioPorCategoriaAsync(int categoriaId);
        Task<List<ServicioCategoriaDto>> ObtenerPreciosCategoriaPorServicioAsync(int servicioId);
        Task<ServicioCategoriaDto?> ObtenerPrecioServicioCategoriaEspecificoAsync(int servicioId, int categoriaId);
    }
}

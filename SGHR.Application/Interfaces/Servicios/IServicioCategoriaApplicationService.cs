using SGHR.Application.DTOs.Servicios;

namespace SGHR.Application.Interfaces.Servicios
{
    public interface IServicioCategoriaApplicationService
    {
        Task AsignarActualizarPrecioServicioCategoriaAsync(AsignarPrecioServicioCategoriaRequest request);
        Task<List<ServicioCategoriaDto>> ObtenerPreciosCategoriaPorServicioAsync(int servicioId);
        Task<ServicioCategoriaDto?> ObtenerPrecioServicioCategoriaEspecificoAsync(int servicioId, int categoriaId);
    }
}

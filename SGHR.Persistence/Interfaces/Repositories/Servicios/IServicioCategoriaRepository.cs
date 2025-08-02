using SGHR.Domain.Entities.Servicios;

namespace SGHR.Persistence.Interfaces.Repositories.Servicios
{
    public interface IServicioCategoriaRepository
    {
        Task AgregarActualizarPrecioServicioCategoriaAsync(int servicioId, int categoriaId, decimal precio);
        Task<List<ServicioCategoria>> ObtenerPreciosPorServicioAsync(int servicioId);
    }
}


namespace SGHR.Persistence.Interfaces.Repositories.Habitaciones
{
    public interface ICategoriaHabitacionRepository
    {
        Task<object?> ObtenerPorIdAsync(int categoriaId);
    }
}

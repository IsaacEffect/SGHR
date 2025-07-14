using SGHR.Domain.Entities.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Base;

namespace SGHR.Persistence.Interfaces.Repositories.Habitaciones
{
    public interface ICategoriaHabitacionRepository : IBaseRepository<CategoriaHabitacion>
    {
        Task<CategoriaHabitacion> ObtenerPorIdAsync(int categoriaId);
        Task<bool> HayDisponibilidadAsync(int idCategoriaH, DateTime fechaEntrada, DateTime FechaSalida, int? reservaid = null);
    }
}

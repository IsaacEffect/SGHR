using SGHR.Domain.Base;
using SGHR.Domain.Entities.Servicios;
namespace SGHR.Domain.Entities.Habitaciones
{
    public class CategoriaHabitacion : EntityBase
    {
        public ICollection<ServicioCategoria> ServicioCategorias { get; set; } = new List<ServicioCategoria>();
    }
}

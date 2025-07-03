using SGHR.Domain.Base;
using SGHR.Domain.Entities.Servicios;
using SGHR.Domain.Enums;
namespace SGHR.Domain.Entities.Habitaciones
{
    public class CategoriaHabitacion : EntityBase
    {
        public ICollection<ServicioCategoria> ServicioCategorias { get; set; } = new List<ServicioCategoria>();
        public string Nombre { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public decimal TarifaBase { get; private set; } = 0.0m;
        public string Caracteristicas { get; private set; } = string.Empty;
        public EstadoHabitacion Estado { get; private set; } 

        
    }
}

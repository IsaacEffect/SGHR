using SGHR.Domain.Base;
using SGHR.Domain.Entities.Habitaciones;

namespace SGHR.Domain.Entities.Servicios
{
    public class ServicioCategoria : AuditableEntity
    {
        public int IdServicio { get; private set; }
        public int IdCategoriaHabitacion { get; private set; }
        public decimal PrecioServicio { get; private set; }
        public Servicios? Servicios { get; private set; }
        public CategoriaHabitacion? CategoriaHabitacion { get; private set; }
        public string NombreServicio { get; set; } 
        public string NombreCategoriaHabitacion { get; set; } 


        protected ServicioCategoria() { }
        public ServicioCategoria(int servicioId, int categoriaHabitacionId, decimal precio, string nombreServicio, string nombreCategoriaHabitacion)
        {
            ValidarParametros(servicioId, categoriaHabitacionId, precio);

            IdServicio = servicioId;
            IdCategoriaHabitacion = categoriaHabitacionId;
            PrecioServicio = precio;
            NombreServicio = nombreServicio;
            NombreCategoriaHabitacion = nombreCategoriaHabitacion;
        }
        private static void ValidarParametros(int servicioId, int categoriaHabitacionId, decimal precio)
        {
            if (servicioId <= 0)
                throw new ArgumentException("El ID del servicio debe ser mayor que cero.", nameof(servicioId));
            if (categoriaHabitacionId <= 0)
                throw new ArgumentException("El ID de la categoría de habitación debe ser mayor que cero.", nameof(categoriaHabitacionId));
            ValidarPrecio(precio);
        }
        private static void ValidarPrecio(decimal precio)
        {
            if (precio < 0)
                throw new ArgumentException("El precio no puede ser negativo.", nameof(precio));
        }
    }
}
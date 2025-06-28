using SGHR.Domain.Base;
using SGHR.Domain.Entities.Habitaciones;
namespace SGHR.Domain.Entities.Servicios
{
    public class ServicioCategoria : AuditableEntity
    {
        public int ServicioId { get; private set; }
        public int CategoriaHabitacionId { get; private set; }
        public decimal Precio { get; private set; }
        public Servicios? Servicios { get; private set; }
        public CategoriaHabitacion? CategoriaHabitacion { get; private set; }

   

        public ServicioCategoria(int servicioId, int categoriaHabitacionId, decimal precio)
        {
            if (servicioId <= 0)
                throw new ArgumentException("El ID del servicio debe ser mayor que cero.", nameof(servicioId));
            if (categoriaHabitacionId <= 0)
                throw new ArgumentException("El ID de la categoría de habitación debe ser mayor que cero.", nameof(categoriaHabitacionId));
            if (precio < 0)
                throw new ArgumentException("El precio no puede ser negativo.", nameof(precio));
            
            ServicioId = servicioId;
            CategoriaHabitacionId = categoriaHabitacionId;
            Precio = precio;

        }
        public void ActualizarPrecio(decimal nuevoPrecio)
        {
            if (nuevoPrecio < 0)
                throw new ArgumentException("El precio no puede ser negativo.", nameof(nuevoPrecio));
            Precio = nuevoPrecio;
        }
    }
}
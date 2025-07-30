using SGHR.Domain.Base;

namespace SGHR.Domain.Entities.Servicios
{
    public class Servicios : AuditableEntity
    {
        public string Nombre { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public bool Eliminado { get; set; } = false;
        public DateTime? FechaEliminacion { get; set; } 

        public ICollection<ServicioCategoria> ServicioCategorias { get; private set; }


        protected Servicios()
        {
            ServicioCategorias = new HashSet<ServicioCategoria>();
        }

        public Servicios(string nombre, string descripcion, bool eliminado, DateTime? fechaEliminacion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción no puede estar vacía.", nameof(descripcion));

            Nombre = nombre;
            Descripcion = descripcion;
            ServicioCategorias = new HashSet<ServicioCategoria>();
            Eliminado = eliminado;
            FechaEliminacion = fechaEliminacion;

        }
        public void Actualizar(string nuevoNombre, string nuevaDescripcion)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nuevoNombre));
            if (string.IsNullOrWhiteSpace(nuevaDescripcion))
                throw new ArgumentException("La descripción no puede estar vacía.", nameof(nuevaDescripcion));
            Nombre = nuevoNombre;
            Descripcion = nuevaDescripcion;
            SetFechaUltimaModificacion();

        }
        public void Eliminar()
        {
            if (Eliminado)
                throw new InvalidOperationException("El servicio ya ha sido eliminado.");

            Eliminado = true;
            FechaEliminacion = DateTime.UtcNow;
            SetFechaUltimaModificacion();
        }


    }
}

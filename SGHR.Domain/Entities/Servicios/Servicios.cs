using SGHR.Domain.Base;

namespace SGHR.Domain.Entities.Servicios
{
    public class Servicios : EntityBase
    {
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public bool Activo { get; private set; }

        
        protected Servicios()
        {
            Nombre = string.Empty; 
            Descripcion = string.Empty; 
            Activo = false; 
        }

        public Servicios(string nombre, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción no puede estar vacía.", nameof(descripcion));

            Nombre = nombre;
            Descripcion = descripcion;
            Activo = true;
        }
        public void Actualizar(string nuevoNombre, string nuevaDescripcion)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nuevoNombre));
            if (string.IsNullOrWhiteSpace(nuevaDescripcion))
                throw new ArgumentException("La descripción no puede estar vacía.", nameof(nuevaDescripcion));
            Nombre = nuevoNombre;
            Descripcion = nuevaDescripcion;
           
        }
        public void Activar()
        {
            Activo = true;
        }
        public void Desactivar()
        {
            Activo = false;
        }

    }
}

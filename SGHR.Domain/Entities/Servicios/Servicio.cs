namespace SGHR.Domain.Entities.Servicios
{
    public class Servicio
    {
        public int IdServicio { get; private set; }
        public string Nombre { get; private set; }
        public decimal PrecioBase { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;
        public bool Activo { get; private set; }

        public Servicio(int id, string nombre, string descripcion, decimal precioBase)
        {
            IdServicio = id;
            Nombre = nombre;
            PrecioBase = precioBase >= 0 ? precioBase : throw new ArgumentException("El precio debe ser positivo.");
            Descripcion = descripcion;
            Activo = true;
        }

        public void Activar() => Activo = true;
        public void Desactivar() => Activo = false;

        public void ActualizarPrecio(decimal nuevoPrecio)
        {
            if (nuevoPrecio < 0)
                throw new ArgumentException("El nuevo precio debe ser positivo.", nameof(nuevoPrecio));
            PrecioBase = nuevoPrecio;
        }

        public void Renombrar(string nuevoNombre)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nuevoNombre));
            Nombre = nuevoNombre;
        }

    }
}

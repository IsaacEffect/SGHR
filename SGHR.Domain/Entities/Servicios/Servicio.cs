using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Entities.Servicios
{
    public class Servicio
    {
        public int IdServicio { get; private set; }
        public string Nombre { get; private set; }
        public decimal PrecioBase { get; private set; }
        public bool Activo { get; private set; }

        public Servicio(int id, string nombre, decimal precioBase)
        {
            IdServicio = id;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            PrecioBase = precioBase >= 0 ? precioBase : throw new ArgumentException("El precio debe ser positivo.");
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

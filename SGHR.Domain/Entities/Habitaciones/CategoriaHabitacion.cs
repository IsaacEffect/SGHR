using SGHR.Domain.Base;
using SGHR.Domain.Entities.Servicios;
using SGHR.Domain.Enums;
namespace SGHR.Domain.Entities.Habitaciones
{
    public class CategoriaHabitacion : EntityBase
    {
        public ICollection<ServicioCategoria> ServicioCategorias { get; set; } = new List<ServicioCategoria>();
        public int Id { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public decimal TarifaBase { get; private set; } = 0.0m;
        public string Caracteristicas { get; private set; } = string.Empty;
        public EstadoHabitacion Estado { get; private set; } 


        public CategoriaHabitacion(int id, string nombre, string descripcion, decimal tarifaBase, string caracteristicas)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
            if (tarifaBase < 0)
                throw new ArgumentException("La tarifa base no puede ser negativa.", nameof(tarifaBase));
            if (string.IsNullOrWhiteSpace(caracteristicas))
                throw new ArgumentException("Las características no pueden estar vacías.", nameof(caracteristicas));
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            TarifaBase = tarifaBase;
            Caracteristicas = caracteristicas;
            Estado = EstadoHabitacion.Disponible;
        }

        public void EstablecerEstado(EstadoHabitacion nuevoEstado)
        {
            if (!Enum.IsDefined(typeof(EstadoHabitacion), nuevoEstado))
                throw new ArgumentException("Estado de habitación no válido.", nameof(nuevoEstado));
            Estado = nuevoEstado;
        }

    }
}

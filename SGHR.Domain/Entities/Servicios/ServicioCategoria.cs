using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Entities.Servicios
{
    public class ServicioCategoria
    {
        public int ServicioId { get; private set; }
        public int CategoriaHabitacionId { get; private set; }
        public decimal PrecioCategoriaServicio { get; private set; }

        public ServicioCategoria(int IdServicio, int categoriaHabitacionId, decimal precioServicio)
        {
            if (precioServicio < 0)
                throw new ArgumentException("El precio del servicio debe ser positivo.");
            IdServicio = ServicioId;
            CategoriaHabitacionId = categoriaHabitacionId;
            PrecioCategoriaServicio = precioServicio;
        }
        public void ActualizarPrecio(decimal nuevoPrecio)
        {
            if (nuevoPrecio < 0)
                throw new ArgumentException("El nuevo precio debe ser positivo.");

            PrecioCategoriaServicio = nuevoPrecio;
        }
    }
}
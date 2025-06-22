using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Model.Dtos
{
    public record HistorialReservaDto
    {
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public string Estado { get; set; }
        public decimal Tarifa { get; set; }
        public string TipoHabitacion { get; set; }
        public string ServiciosAdicionales { get; set; }
    }
}

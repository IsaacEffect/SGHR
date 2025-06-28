using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.DTOsTarifa
{
    public record class ActualizarTarifaDto
    {
        public int IdCategoriaHabitacion { get; set; }
        public string TipoTemporada { get; set; } = string.Empty;
        public decimal Precio { get; set; }
       
    }
}

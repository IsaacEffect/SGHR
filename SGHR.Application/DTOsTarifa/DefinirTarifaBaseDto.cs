using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.DTOsTarifa
{
    public record class DefinirTarifaBaseDto
    {
        public int IdCategoriaHabitacion { get; set; }
        public decimal Precio { get; set; }
        public string TipoTemporada { get; set; } = string.Empty;


    }
}

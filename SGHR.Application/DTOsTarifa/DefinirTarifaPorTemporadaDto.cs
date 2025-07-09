using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.DTOsTarifa
{
    public record class DefinirTarifaPorTemporadaDto
    {
        [Required]
        public DateTime? FechaInicio { get; set; }

        [Required]
        public DateTime? FechaFin { get; set; }

        [Required]
        public string TipoTemporada { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }

        
       
    }
}

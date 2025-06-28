using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.DTOs
{
    public record ActualizarCategoriaDto
    {

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public decimal TarifaBase { get; set; }

        public string? Caracteristicas { get; set; }

        public bool Estado { get; set; }
    }
}

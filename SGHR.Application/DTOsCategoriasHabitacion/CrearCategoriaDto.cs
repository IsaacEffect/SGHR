using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.DTOs
{
    public record CrearCategoriaDto
    {
        [Required]
        public string Nombre { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "La descripcion no pueden exceder los 500 caracteres.")]
        public string? Descripcion { get; set; }

        [MaxLength(100, ErrorMessage = "Las características no pueden exceder los 100 caracteres.")]
        public string? Caracteristicas { get; set; }
    }
}

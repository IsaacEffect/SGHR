using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionHotel.Domain.Entities
{
    public class Piso
    {
        public int Id { get; set; }               // Identificador único del piso
        public string Nombre { get; set; }        // Ej: "Piso 1", "Piso 2"
        public int Nivel { get; set; }            // Nivel numérico (ej. 1, 2, 3)
    }
}


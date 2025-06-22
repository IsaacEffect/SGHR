using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence
{
    public class Piso
    {
        public int Id { get; set; }  
        public string Numero { get; set; } 
        public ICollection<Habitacion> Habitaciones { get; set; }  
    }
}

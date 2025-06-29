using SGHR.Domain.Entities.Historial;

namespace SGHR.Domain.Entities.Clientes
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Rol { get; set; }

        public ICollection<HistorialReserva>? Historial { get; set; }
    }
}

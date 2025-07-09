using SGHR.Domain.Entities.Clientes;

namespace SGHR.Domain.Entities.Historial
{
    public class HistorialReserva
    {
        public int Id { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public string Estado { get; set; } // Confirmada, Cancelada, etc.
        public decimal Tarifa { get; set; }
        public string TipoHabitacion { get; set; }
        public string ServiciosAdicionales { get; set; }

        // Relación con Cliente
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}

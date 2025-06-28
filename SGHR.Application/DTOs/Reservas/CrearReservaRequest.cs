using SGHR.Domain.enums;

namespace SGHR.Application.DTOs.Reservas
{
    public class CrearReservaRequest
    {
        public int ClienteId { get; set; }
        public int IdCategoriaHabitacion { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; } 
        public EstadoReserva Estado { get; set; }
        public sbyte NumeroHuespedes { get; set; }
    }
}

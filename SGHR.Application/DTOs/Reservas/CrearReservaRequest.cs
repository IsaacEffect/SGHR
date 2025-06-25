namespace SGHR.Application.DTOs.Reservas
{
    public class CrearReservaRequest
    {
        public int ClienteId { get; set; }
        public int IdCategoriaHabitacion { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; } 
        public int NumeroHuespedes { get; set; } 
    }
}

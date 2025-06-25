namespace SGHR.Application.DTOs.Reservas
{
    public class ActualizarReservaRequest
    {
        public int IdReserva { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public int? NumeroHuespedes { get; set; }
    }
}

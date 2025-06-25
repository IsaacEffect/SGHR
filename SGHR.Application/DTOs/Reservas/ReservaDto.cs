namespace SGHR.Application.DTOs.Reservas
{
    public class ReservaDto
    {
        public int IdReserva { get; set; }
        public int ClienteId { get; set; }
        public int IdCategoriaHabitacion { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}

namespace SGHR.Application.DTOs.Reservas
{
    public class CancelarReservaDto
    {
        public int IdReserva { get; set; } 
        public string MotivoCancelacion { get; set; } = string.Empty;
    }
}


namespace SGHR.Application.DTOs.Reservas
{
    public class VerificarDisponibilidadRequest
    {
        public int IdCategoriaHabitacion { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        
    }
}

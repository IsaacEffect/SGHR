using SGHR.Domain.Enums;
namespace SGHR.Application.DTOs.Reservas
{
    public class ReservaDto
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdCategoriaHabitacion { get; set; }
        public int NumeroHuespedes { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public EstadoReserva Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}

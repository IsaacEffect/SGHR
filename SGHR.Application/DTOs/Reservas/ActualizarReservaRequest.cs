using SGHR.Domain.enums;
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.DTOs.Reservas
{
    public class ActualizarReservaRequest
    {
        [Required(ErrorMessage = "El Id de la reserva es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id de la reserva debe ser un numero positivo")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El Id de la categoria de habitacion es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id de la categoria de habitacion debe ser un numero positivo")]
        public int IdCategoriaHabitacion { get; set; }
        
        [Required(ErrorMessage = "La fecha de entrada es requerida")]
        public DateTime FechaEntrada { get; set; }
        
        [Required(ErrorMessage = "La fecha de salida es requerida")]
        public DateTime FechaSalida { get; set; }
        public EstadoReserva Estado { get; set; }
        
        [Required(ErrorMessage = "El numero de huespedes es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El numero de huespedes debe ser al menos 1 y no exceder 255")]
        public sbyte NumeroHuespedes { get; set; }

    }
}

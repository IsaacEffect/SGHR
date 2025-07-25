using SGHR.Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace SGHR.Web.ViewModel.Reservas
{
    public class ActualizarReservaViewModel
    {
        [Required(ErrorMessage = "El Id de la reserva es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id de la reserva debe ser un número positivo")]
        public string IdCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Id de la categoría de habitación es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id de la categoría de habitación debe ser un número positivo")]
        public string IdCategoriaHabitacion { get; set; } = string.Empty;

        [Required(ErrorMessage ="El numero de huéspedes es requerido")]
        [Range(1, 255, ErrorMessage = "El número de huéspedes debe ser al menos 1 y no exceder 255")]
        public int NumeroHuespedes { get; set; }

        [Required(ErrorMessage = "La fecha de entrada es requerida")]
        [DataType(DataType.Date, ErrorMessage = "El formato de FechaEntrada no es válido.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaEntrada { get; set; }

        [Required(ErrorMessage = "La fecha de salida es requerida")]
        [DataType(DataType.Date, ErrorMessage = "El formato de FechaSalida no es válido.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaSalida { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [EnumDataType(typeof(EstadoReserva), ErrorMessage = "El estado debe ser un valor válido de EstadoReserva.")]
        public EstadoReserva Estado { get; set; }

        public int Activo { get; set; }



    }
}

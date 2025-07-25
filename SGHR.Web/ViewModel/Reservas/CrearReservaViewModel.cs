using Microsoft.AspNetCore.Mvc.Rendering;
using SGHR.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SGHR.Web.ViewModel.Reservas
{
    public class CrearReservaViewModel
    {
        [JsonPropertyName("clienteId")]
        [Required(ErrorMessage = "El campo IdCliente es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo IdCliente debe ser un numero valido.")]
        public int IdCliente { get; set; }

        [JsonPropertyName("idCategoriaHabitacion")]
        [Required(ErrorMessage = "El campo IdCategoriaHabitacion es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo IdCategoriaHabitacion debe ser un número positivo.")]
        public int IdCategoriaHabitacion { get; set; }

        [JsonPropertyName("numeroHuespedes")]
        [Required(ErrorMessage = "El campo NumeroHuespedes es obligatorio.")]
        [Range(1, 10, ErrorMessage = "El campo NumeroHuespedes debe ser un número entre 1 y 10.")]
        public int NumeroHuespedes { get; set; }

        [JsonPropertyName("fechaEntrada")]
        [Required(ErrorMessage = "El campo FechaEntrada es obligatorio.")]
        [DataType(DataType.Date, ErrorMessage = "El formato de FechaEntrada no es válido.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaEntrada { get; set; }

        [JsonPropertyName("fechaSalida")]
        [Required(ErrorMessage = "El campo FechaSalida es obligatorio.")]
        [DataType(DataType.Date, ErrorMessage = "El formato de FechaSalida no es válido.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaSalida { get; set; }

        [Required(ErrorMessage = "El campo de Estado es necesario")]

        [JsonPropertyName("estado")]
        public EstadoReserva EstadoReserva { get; set; }

    }
}

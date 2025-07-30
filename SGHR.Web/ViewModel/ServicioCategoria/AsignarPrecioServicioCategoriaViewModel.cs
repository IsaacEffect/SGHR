using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SGHR.Web.ViewModel.ServicioCategoria
{
    public class AsignarPrecioServicioCategoriaViewModel
    {
        [JsonPropertyName("idServicio")]
        [Required]
        public int IdServicio { get; set; }

        [JsonIgnore]
        public string? NombreServicio { get; set; }

        [JsonPropertyName("idCategoriaHabitacion")]
        [Display(Name = "Categoría de Habitación")]
        [Required(ErrorMessage = "Debe seleccionar una categoría de habitación.")]
        public int IdCategoriaHabitacion { get; set; }

        [JsonPropertyName("precio")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

    }
}

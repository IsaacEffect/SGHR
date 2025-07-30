using System.Text.Json.Serialization;

namespace SGHR.Web.ViewModel.ServicioCategoria
{
    public class ServicioCategoriaViewModel 
    {
        [JsonPropertyName("idServicio")] 
        public int IdServicio { get; set; }

        [JsonPropertyName("idCategoriaHabitacion")]
        public int IdCategoriaHabitacion { get; set; }

        [JsonPropertyName("precioServicio")] 
        public decimal PrecioServicio { get; set; }

        [JsonPropertyName("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("fechaModificacion")]
        public DateTime? FechaModificacion { get; set; }

        [JsonPropertyName("nombreServicio")] 
        public string? NombreServicio { get; set; }

        [JsonPropertyName("nombreCategoriaHabitacion")]
        public string? NombreCategoriaHabitacion { get; set; }
    }
}
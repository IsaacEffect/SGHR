using System.Text.Json.Serialization;

namespace SGHR.Application.DTOs.Servicios
{
    public class AsignarPrecioServicioCategoriaRequest
    {
        [JsonPropertyName("idServicio")]
        public int IdServicio { get; set; }

        [JsonPropertyName("idCategoriaHabitacion")]
        public int IdCategoriaHabitacion { get; set; }

        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }

    }
}

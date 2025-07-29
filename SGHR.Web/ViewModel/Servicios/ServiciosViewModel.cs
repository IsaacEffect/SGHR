using System.Text.Json.Serialization;

namespace SGHR.Web.ViewModel.Servicios
{
    public class ServiciosViewModel
    {
        [JsonPropertyName("idServicio")]
        public int IdServicio { get; set; } 

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("activo")]
        public bool Activo { get; set; }
    }
}

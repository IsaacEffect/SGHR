using System.Text.Json.Serialization;

namespace SGHR.Web.ViewModel.Servicios
{
    public class CrearServiciosViewModel
    {
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("activo")]
        public bool Activo { get; set; }
    }
}

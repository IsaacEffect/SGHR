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

        [JsonPropertyName("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("fechaModificacion")] 
        public DateTime? FechaModificacion { get; set; }

        [JsonPropertyName("eliminado")] 
        public bool Eliminado { get; set; }

        [JsonPropertyName("fechaEliminacion")] 
        public DateTime? FechaEliminacion { get; set; }


    }
}

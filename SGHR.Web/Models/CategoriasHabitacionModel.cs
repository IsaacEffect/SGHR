using System.Text.Json.Serialization;

namespace SGHR.Web.Models
{
    public class CategoriaHabitacion
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("caracteristicas")]
        public string Caracteristicas { get; set; }

        [JsonPropertyName("tarifaBase")]
        public decimal TarifaBase { get; set; }

        [JsonPropertyName("estado")]
        public bool Estado { get; set; }

        public CategoriaHabitacion() { }
    }

    public class CategoriasHabitacionModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string? descripcion { get; set; }

        [JsonPropertyName("caracteristicas")]
        public string? caracteristicas { get; set; }

        [JsonPropertyName("tarifaBase")]
        public decimal tarifaBase { get; set; }

        [JsonPropertyName("estado")]
        public bool estado { get; set; }

        [JsonIgnore]
        public string EstadoTexto => estado ? "Activo" : "Inactivo";
    }

}

namespace SGHR.Application.DTOs.Servicios
{
    public class ServicioCategoriaDto
    {
        public int IdServicio { get; set; }
        public string NombreServicio { get; set; } = string.Empty;
        public int IdCategoriaHabitacion { get; set; }
        public string NombreCategoriaHabitacion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
    }
}

namespace SGHR.Application.DTOs.Servicios
{
    public class ServicioCategoriaDto
    {
        public int IdServicio { get; set; }
        public int IdCategoriaHabitacion { get; set; }
        public decimal PrecioServicio { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string? NombreServicio { get; set; }
        public string? NombreCategoriaHabitacion { get; set; }

    }
}

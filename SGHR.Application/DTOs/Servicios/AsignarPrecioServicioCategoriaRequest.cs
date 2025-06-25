namespace SGHR.Application.DTOs.Servicios
{
    public class AsignarPrecioServicioCategoriaRequest
    {
        public int IdServicio { get; set; }
        public int IdCategoriaHabitacion { get; set; }
        public decimal Precio { get; set; }

    }
}

namespace SGHR.Application.DTOs.Servicios
{
    public class ActualizarServicioRequest
    {
        public int IdServicio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;

    }
}

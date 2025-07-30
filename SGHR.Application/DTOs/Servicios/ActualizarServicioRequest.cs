namespace SGHR.Application.DTOs.Servicios
{
    public class ActualizarServicioRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;

    }
}

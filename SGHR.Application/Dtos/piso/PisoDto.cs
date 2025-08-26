namespace SGHR.Application.Dtos
{
    public class PisoDto
    {
        public int Id { get; set; }
        public string NumeroPiso { get; set; } = string.Empty;   // 🔥 evita CS8618
        public string Descripcion { get; set; } = string.Empty;  // 🔥 evita CS8618
    }
}

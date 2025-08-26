namespace SGHR.Application.Dtos
{
    public class UpdatePisoDto
    {
        public int Id { get; set; }
        public string NumeroPiso { get; set; } = string.Empty;   // 🔥 requerido
        public string Descripcion { get; set; } = string.Empty;  // 🔥 requerido
    }
}

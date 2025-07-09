using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos
{
    public record CambiarContrasenaDto
    {
        [Required]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Debe ingresar su contraseña actual.")]
        [StringLength(100)]
        public string ContrasenaActual { get; set; }

        [Required(ErrorMessage = "Debe ingresar una nueva contraseña.")]
        [StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "La nueva contraseña debe tener mínimo una letra y un número.")]
        public string NuevaContrasena { get; set; }
    }
}

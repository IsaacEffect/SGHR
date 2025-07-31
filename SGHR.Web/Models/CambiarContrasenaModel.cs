using System.ComponentModel.DataAnnotations;

namespace SGHR.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CambiarContrasenaModel
    {
        [Required]
        public string contrasenaActual { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string nuevaContrasena { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("nuevaContrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        public string confirmarNuevaContrasena { get; set; }
    }


}

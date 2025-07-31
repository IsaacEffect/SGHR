using System.ComponentModel.DataAnnotations;

namespace SGHR.Web.Models
{
    public class ActualizarClienteModel
    {
        public int idCliente { get; set; }

        [Required]
        public string nombre { get; set; }

        [Required]
        public string apellido { get; set; }

        [Required]
        public string email { get; set; }
        public string direccion { get; set; }

        [Required]
        public string telefono { get; set; }
    }
}

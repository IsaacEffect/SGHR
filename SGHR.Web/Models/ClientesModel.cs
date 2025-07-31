namespace SGHR.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ClientesModel
    {
        public int idCliente { get; set; }

        [Required]
        public string nombre { get; set; }

        [Required]
        public string apellido { get; set; }

        [Required, EmailAddress]
        public string email { get; set; }

        [Required]
        public string telefono { get; set; }

        public string direccion { get; set; }

        public DateTime fechaRegistro { get; set; }

        //public string Rol { get; set; }

        [Required, MinLength(6)]
        public string contrasena { get; set; }

        // para el formulario
        [Compare("contrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmarContrasena { get; set; }
    }


    public class ObtenerTodosClientesResponse
    {
        public bool success { get; set; }
        public object message { get; set; }
        public List<ClientesModel> data { get; set; }
    }

    public class ObtenerClienteResponse
    {
        public bool success { get; set; }
        public object message { get; set; }
        public ClientesModel data { get; set; }
    }

}


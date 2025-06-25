using SGHR.Domain.Base;
using SGHR.Domain.enums;

namespace SGHR.Domain.Entities.Users
{
    public class Usuario : EntityBase
    {
        public string NombreUsuario { get; private set; } = string.Empty; 
        public string HashedPassword { get; private set; } = string.Empty; 
        public string Email { get; private set; } = string.Empty; 
        public RolUsuario Rol { get; private set; }

        public string? NombreCompleto { get; private set; }
        public string? Telefono { get; private set; }
        public string? Direccion { get; private set; }

        public bool Activo { get; private set; }
        public DateTime FechaRegistro { get; private set; }
        public DateTime? FechaUltimoAcceso { get; private set; }

        public Usuario(string nombreUsuario, string hashedPassword, string email, RolUsuario rol, string? nombreCompleto = null,
                    string? telefono = null, string? direccion = null)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario)) throw new ArgumentException("El nombre de usuario es requerido.");
            if (string.IsNullOrWhiteSpace(hashedPassword)) throw new ArgumentException("El hash de la contraseña es requerido.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("El email es requerido.");

            NombreUsuario = nombreUsuario;
            HashedPassword = hashedPassword;
            Email = email;
            Rol = rol;
            NombreCompleto = nombreCompleto;
            Telefono = telefono;
            Direccion = direccion;
            Activo = true;
            FechaRegistro = DateTime.UtcNow;
        }

        protected Usuario()
        {
           
            NombreUsuario = string.Empty;
            HashedPassword = string.Empty;
            Email = string.Empty;
            Rol = default!;
            Activo = false;
            FechaRegistro = DateTime.MinValue;
        }

        public void ActualizarInformacion(string? nombreCompleto = null, string? telefono = null, string? direccion = null)
        {
            if (!string.IsNullOrWhiteSpace(nombreCompleto)) NombreCompleto = nombreCompleto;
            if (!string.IsNullOrWhiteSpace(telefono)) Telefono = telefono;
            if (!string.IsNullOrWhiteSpace(direccion)) Direccion = direccion;
        }
    }
}

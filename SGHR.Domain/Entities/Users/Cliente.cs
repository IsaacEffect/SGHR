using SGHR.Domain.Base;
using SGHR.Domain.enums;

namespace SGHR.Domain.Entities.Users
{
    public class Cliente : AuditableEntity
    {
        public string NombreUsuario { get; private set; } = string.Empty; 
        public string HashedPassword { get; private set; } = string.Empty; 
        public string Email { get; private set; } = string.Empty; 
        public RolUsuario Rol { get; private set; }
        public string? NombreCompleto { get; private set; }
        public string? Telefono { get; private set; }
        public string? Direccion { get; private set; }

        protected Cliente() { }

        public Cliente(string nombreUsuario, string hashedPassword, string email, RolUsuario rol, string? nombreCompleto = null,
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
        }

        public void ActualizarPerfil(string? nombreCompleto, string? telefono, string? direccion, string email)
        {
            NombreCompleto = nombreCompleto;
            Telefono = telefono;
            Direccion = direccion;
            Email = email;
            SetFechaUltimaModificacion();
        }

        public void CambiarPassword(string nuevoHashedPassword)
        {
            if (string.IsNullOrWhiteSpace(nuevoHashedPassword))
                throw new ArgumentException("El nuevo hash de la contraseña es requerido.", nameof(nuevoHashedPassword));
            HashedPassword = nuevoHashedPassword;
            SetFechaUltimaModificacion();
        }
        
    }
}

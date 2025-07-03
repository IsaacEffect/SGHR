using SGHR.Domain.Base;
using SGHR.Domain.enums;

namespace SGHR.Domain.Entities.Users
{
    public class Cliente : AuditableEntity
    {
        public string Nombre { get; private set; } = string.Empty;
        public string Apellido { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string? Telefono { get; private set; }
        public string? Direccion { get; private set; }
        public string HashedPassword { get; private set; } = string.Empty;
        public RolUsuario Rol { get; private set; }

        protected Cliente() { }

        public Cliente(string nombre, string hashedPassword, string email, RolUsuario rol, string? apellido,
                    string? telefono = null, string? direccion = null)
        {
            if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es requerido.");
            if (string.IsNullOrWhiteSpace(apellido)) throw new ArgumentException("El apellido es requerido."); 
            if (string.IsNullOrWhiteSpace(hashedPassword)) throw new ArgumentException("La contraseña es requerida.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("El email es requerido.");

            Nombre = nombre;
            HashedPassword = hashedPassword;
            Email = email;
            Rol = rol;
            Apellido = apellido;
            Telefono = telefono;
            Direccion = direccion;
            SetFechaUltimaModificacion();
        }

        public void ActualizarPerfil(string? nombre, string? apellido, string? telefono, string? direccion, string email)
        {
            Nombre = nombre ?? Nombre; 
            Apellido = apellido;
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

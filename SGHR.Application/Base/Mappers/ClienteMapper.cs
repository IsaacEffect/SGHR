using SGHR.Application.Dtos;
using SGHR.Domain.Entities.Clientes;

namespace SGHR.Application.Base.Mappers
{
    public static class ClienteMapper
    {
        public static ObtenerClienteDto ToDto(this Cliente cliente)
        {
            return new ObtenerClienteDto
            {
                IdCliente = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Email = cliente.Correo,
                Telefono = cliente.Telefono,
                Direccion = cliente.Direccion,
                FechaRegistro = cliente.FechaRegistro
            };
        }

        public static IEnumerable<ObtenerClienteDto> ToDtoList(this IEnumerable<Cliente> clientes)
        {
            return clientes.Select(c => c.ToDto());
        }

        public static Cliente ToEntity(this InsertarClienteDto dto, string contrasenaHasheada)
        {
            return new Cliente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Email,
                Contrasena = contrasenaHasheada,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                FechaRegistro = DateTime.UtcNow,
                Rol = "Cliente"
            };
        }

        public static void MapToExisting(this ModificarClienteDto dto, Cliente cliente)
        {
            cliente.Nombre = dto.Nombre;
            cliente.Apellido = dto.Apellido;
            cliente.Correo = dto.Correo;
            cliente.Direccion = dto.Direccion;
            cliente.Telefono = dto.Telefono;
        }
    }
}

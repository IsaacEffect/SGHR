using Microsoft.Extensions.Logging;
using SGHR.Domain.Entities.Clientes;
using SGHR.Domain.Interfaces;
using SGHR.Domain.Interfaces.Service;
using SGHR.Model.Dtos;

namespace SGHR.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(IUnitOfWork unitOfWork, ILogger<ClienteService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<ObtenerClienteDto>> ObtenerTodosAsync()
        {
            var clientes = await _unitOfWork.Clients.GetAllAsync();

            return clientes.Select(c => new ObtenerClienteDto
            {
                IdCliente = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Email = c.Correo,
                Telefono = c.Telefono,
                Direccion = c.Direccion,
                FechaRegistro = c.FechaRegistro
            });
        }

        public async Task<ObtenerClienteDto> ObtenerPorIdAsync(int id)
        {
            var cliente = await _unitOfWork.Clients.GetByIdAsync(id);

            if (cliente == null)
                return null;

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

        public async Task InsertarAsync(InsertarClienteDto dto)
        {
            var nuevoCliente = new Cliente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Email,
                Contrasena = dto.ContrasenaHashed,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                FechaRegistro = DateTime.UtcNow,
                Rol = "Cliente" // Rol por defecto, se puede cambiar
            };

            await _unitOfWork.Clients.AddAsync(nuevoCliente);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Cliente creado: {Email}", dto.Email);
        }

        public async Task ModificarAsync(ModificarClienteDto dto)
        {
            var cliente = await _unitOfWork.Clients.GetByIdAsync(dto.Id);
            if (cliente == null)
                throw new Exception("Cliente no encontrado.");

            cliente.Nombre = dto.Nombre;
            cliente.Apellido = dto.Apellido;
            cliente.Correo = dto.Correo;
            cliente.Direccion = dto.Direccion;
            cliente.Telefono = dto.Telefono;

            await _unitOfWork.Clients.UpdateAsync(cliente);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Cliente modificado: {Id}", dto.Id);
        }

        public async Task EliminarAsync(int id)
        {
            await _unitOfWork.Clients.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Cliente eliminado: {Id}", id);
        }
    }
}


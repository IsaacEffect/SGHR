using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Base.Mappers;
using SGHR.Application.Contracts.Service;
using SGHR.Application.Dtos;
using SGHR.Domain.Base;
using SGHR.Domain.Interfaces;

namespace SGHR.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ClienteService> _logger;
        private readonly PasswordHasher<string> _passwordHasher = new();

        public ClienteService(IUnitOfWork unitOfWork, ILogger<ClienteService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<ObtenerClienteDto>>> ObtenerTodosAsync()
        {
            return await ServiceExecutor.ExecuteAsync(_logger, async () =>
            {
                var clientes = await _unitOfWork.Clients.GetAllAsync();
                return clientes.ToDtoList();
            }, "Error inesperado al obtener los clientes.");
        }

        public async Task<OperationResult<ObtenerClienteDto>> ObtenerPorIdAsync(int id)
        {
            return await ServiceExecutor.ExecuteAsync(_logger, async () =>
            {
                var cliente = await _unitOfWork.Clients.GetByIdAsync(id);
                if (cliente == null)
                    throw new InvalidOperationException("Cliente no encontrado.");

                return cliente.ToDto();
            }, "Error inesperado al buscar el cliente.", $"ClienteId: {id}");
        }

        public async Task<OperationResult> InsertarAsync(InsertarClienteDto dto)
        {
            return await ServiceExecutor.ExecuteAsync(_logger, async () =>
            {
                var existente = await _unitOfWork.Clients.GetByEmailAsync(dto.Email);
                if (existente != null)
                    throw new InvalidOperationException("El correo ya está registrado por otro cliente.");

                var hash = _passwordHasher.HashPassword(null, dto.ContrasenaHashed);
                var nuevoCliente = dto.ToEntity(hash);

                await _unitOfWork.Clients.AddAsync(nuevoCliente);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Cliente creado: {Email}", dto.Email);
            }, "Error al crear cliente.", $"Email: {dto.Email}");
        }

        public async Task<OperationResult> ModificarAsync(ModificarClienteDto dto)
        {
            return await ServiceExecutor.ExecuteAsync(_logger, async () =>
            {
                var cliente = await _unitOfWork.Clients.GetByIdAsync(dto.Id);
                if (cliente == null)
                    throw new InvalidOperationException("Cliente no encontrado.");

                if (!string.Equals(cliente.Correo, dto.Correo, StringComparison.OrdinalIgnoreCase))
                {
                    var otroConEseCorreo = await _unitOfWork.Clients.GetByEmailAsync(dto.Correo);
                    if (otroConEseCorreo != null && otroConEseCorreo.Id != cliente.Id)
                        throw new InvalidOperationException("Ese correo ya está en uso por otro cliente.");
                }

                dto.MapToExisting(cliente);
                await _unitOfWork.Clients.UpdateAsync(cliente);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Cliente modificado: {Id}", dto.Id);
            }, "Error al modificar cliente.", $"ClienteId: {dto.Id}");
        }

        public async Task<OperationResult> CambiarContrasenaAsync(CambiarContrasenaDto dto)
        {
            return await ServiceExecutor.ExecuteAsync(_logger, async () =>
            {
                var cliente = await _unitOfWork.Clients.GetByIdAsync(dto.IdCliente);
                if (cliente == null)
                    throw new InvalidOperationException("Cliente no encontrado.");

                var resultadoHash = _passwordHasher.VerifyHashedPassword(null, cliente.Contrasena, dto.ContrasenaActual);
                if (resultadoHash == PasswordVerificationResult.Failed)
                    throw new InvalidOperationException("La contraseña actual es incorrecta.");

                cliente.Contrasena = _passwordHasher.HashPassword(null, dto.NuevaContrasena);

                await _unitOfWork.Clients.UpdateAsync(cliente);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Contraseña actualizada para cliente {Id}", dto.IdCliente);
            }, "Error al cambiar la contraseña.", $"ClienteId: {dto.IdCliente}");
        }

        public async Task<OperationResult> EliminarAsync(int id)
        {
            return await ServiceExecutor.ExecuteAsync(_logger, async () =>
            {
                var cliente = await _unitOfWork.Clients.GetByIdAsync(id);
                if (cliente == null)
                    throw new InvalidOperationException("Cliente no encontrado.");

                await _unitOfWork.Clients.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Cliente eliminado: {Id}", id);
            }, "Error al eliminar cliente.", $"ClienteId: {id}");
        }

        public async Task<OperationResult<ObtenerClienteDto>> ValidarCredencialesAsync(string email, string contrasena)
        {
            return await ServiceExecutor.ExecuteAsync(_logger, async () =>
            {
                var cliente = await _unitOfWork.Clients.GetByEmailAsync(email);
                if (cliente == null)
                    throw new InvalidOperationException("Credenciales inválidas.");

                var resultadoHash = _passwordHasher.VerifyHashedPassword(null, cliente.Contrasena, contrasena);
                if (resultadoHash == PasswordVerificationResult.Failed)
                    throw new InvalidOperationException("Credenciales inválidas.");

                return cliente.ToDto();
            }, "Error al validar las credenciales.", $"Email: {email}");
        }
    }
}

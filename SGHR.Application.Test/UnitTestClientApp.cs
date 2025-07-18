using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Application.Dtos;
using SGHR.Application.Services;
using SGHR.Domain.Base;
using SGHR.Domain.Interfaces;
using SGHR.Persistence;
using SGHR.Persistence.Base;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories;

namespace SGHR.Application.Test
{
    public class UnitTestClientApp
    {
        private HotelReservaDBContext CrearContextoInMemory()
        {
            var options = new DbContextOptionsBuilder<HotelReservaDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new HotelReservaDBContext(options);
        }

        private ClienteService CrearServicioConContexto(HotelReservaDBContext context)
        {
            var config = new ConfigurationBuilder().Build();
            var historialLogger = LoggerFactory.Create(builder => builder.AddDebug()).CreateLogger<HistorialReservaRepository>();
            var clienteLogger = LoggerFactory.Create(builder => builder.AddDebug()).CreateLogger<ClienteService>();
            ISqlHelper sqlHelper = new SqlHelperService();

            IUnitOfWork unit = new UnitOfWork(context, config, historialLogger, sqlHelper);
            return new ClienteService(unit, clienteLogger);
        }

        [Fact]
        public async Task InsertarAsync_DeberiaInsertarClienteCorrectamente()
        {
            var context = CrearContextoInMemory();
            var service = CrearServicioConContexto(context);

            var dto = new InsertarClienteDto
            {
                Nombre = "Ana",
                Apellido = "Martinez",
                Email = "ana@test.com",
                ContrasenaHashed = "Test1234",
                Direccion = "Calle Falsa",
                Telefono = "8091234567"
            };

            var result = await service.InsertarAsync(dto);

            Assert.True(result.Success);
            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.Correo == dto.Email);
            Assert.NotNull(cliente);
            Assert.Equal("Ana", cliente.Nombre);
        }

        [Fact]
        public async Task InsertarAsync_DeberiaFallarSiCorreoYaExiste()
        {
            var context = CrearContextoInMemory();
            var service = CrearServicioConContexto(context);

            context.Clientes.Add(new Domain.Entities.Clientes.Cliente
            {
                Nombre = "Existente",
                Apellido = "Cliente",
                Correo = "existe@test.com",
                Contrasena = "hash",
                Direccion = "Calle 1",
                Telefono = "8090000000",
                FechaRegistro = DateTime.UtcNow,
                Rol = "Cliente",
                Estado = true
            });
            await context.SaveChangesAsync();

            var dto = new InsertarClienteDto
            {
                Nombre = "Nuevo",
                Apellido = "Cliente",
                Email = "existe@test.com",
                ContrasenaHashed = "Pass1234",
                Direccion = "Otra Calle",
                Telefono = "8091111111"
            };

            var result = await service.InsertarAsync(dto);
            Assert.False(result.Success);
            Assert.Equal("Error al crear cliente.", result.Message);
        }

        [Fact]
        public async Task EliminarAsync_DeberiaMarcarEstadoFalse()
        {
            var context = CrearContextoInMemory();
            var service = CrearServicioConContexto(context);

            var cliente = new Domain.Entities.Clientes.Cliente
            {
                Nombre = "Eliminar",
                Apellido = "Cliente",
                Correo = "eliminar@test.com",
                Contrasena = "hash",
                Direccion = "Calle",
                Telefono = "8092222222",
                FechaRegistro = DateTime.UtcNow,
                Rol = "Cliente",
                Estado = true
            };
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var result = await service.EliminarAsync(cliente.Id);

            Assert.True(result.Success);
            var eliminado = await context.Clientes.FindAsync(cliente.Id);
            Assert.False(eliminado.Estado);
        }

        [Fact]
        public async Task ModificarAsync_DeberiaActualizarCliente()
        {
            var context = CrearContextoInMemory();
            var service = CrearServicioConContexto(context);

            var cliente = new Domain.Entities.Clientes.Cliente
            {
                Nombre = "Original",
                Apellido = "Apellido",
                Correo = "modificar@test.com",
                Contrasena = "hash",
                Direccion = "Direccion 1",
                Telefono = "8093333333",
                FechaRegistro = DateTime.UtcNow,
                Rol = "Cliente",
                Estado = true
            };
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var dto = new ModificarClienteDto
            {
                Id = cliente.Id,
                Nombre = "Modificado",
                Apellido = "NuevoApellido",
                Correo = "modificar@test.com",
                Direccion = "Direccion 2",
                Telefono = "8099999999"
            };

            var result = await service.ModificarAsync(dto);

            Assert.True(result.Success);
            var actualizado = await context.Clientes.FindAsync(cliente.Id);
            Assert.Equal("Modificado", actualizado.Nombre);
        }

        [Fact]
        public async Task ValidarCredencialesAsync_DeberiaRetornarClienteValido()
        {
            var context = CrearContextoInMemory();
            var service = CrearServicioConContexto(context);

            var hasher = new PasswordHasher<string>();
            var contrasena = "Test1234";
            var hash = hasher.HashPassword(null, contrasena);

            var cliente = new Domain.Entities.Clientes.Cliente
            {
                Nombre = "Login",
                Apellido = "Test",
                Correo = "login@test.com",
                Contrasena = hash,
                Direccion = "Calle",
                Telefono = "8090000000",
                FechaRegistro = DateTime.UtcNow,
                Rol = "Cliente",
                Estado = true
            };
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var result = await service.ValidarCredencialesAsync("login@test.com", contrasena);

            Assert.True(result.Success);
            Assert.Equal("Login", result.Data.Nombre);
        }
    }
}
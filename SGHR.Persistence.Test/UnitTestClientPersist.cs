using Microsoft.EntityFrameworkCore;
using SGHR.Domain.Entities.Clientes;
using SGHR.Persistence.Context;

namespace SGHR.Persistence.Test
{
    public class UnitTestClientPersist
    {
        private HotelReservaDBContext CrearContextoInMemory()
        {
            var options = new DbContextOptionsBuilder<HotelReservaDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new HotelReservaDBContext(options);
        }

        private Cliente CrearClienteValido(string nombreBase, string email = null)
        {
            return new Cliente
            {
                Nombre = nombreBase,
                Apellido = $"Apellido{nombreBase}",
                Correo = email ?? $"{nombreBase.ToLower()}@test.com",
                Contrasena = $"Contrasena{nombreBase}123",
                Direccion = $"Calle {nombreBase} 123",
                Telefono = $"809{new Random().Next(1000000, 9999999)}",
                FechaRegistro = DateTime.UtcNow,
                Rol = "Cliente",
                Estado = true
            };
        }

        [Fact]
        public async Task AddAsync_DeberiaAgregarCliente()
        {
            // Arrange
            var context = CrearContextoInMemory();
            var repo = new ClienteRepository(context);
            var nuevoCliente = CrearClienteValido("Ana", "ana@test.com");

            // Act
            await repo.AddAsync(nuevoCliente);
            await context.SaveChangesAsync();
            var resultado = await context.Clientes.FirstOrDefaultAsync(c => c.Correo == "ana@test.com");

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Ana", resultado.Nombre);
        }

        [Fact]
        public async Task GetByIdAsync_DeberiaDevolverCliente()
        {
            var context = CrearContextoInMemory();
            var cliente = CrearClienteValido("Juan");
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var repo = new ClienteRepository(context);
            var resultado = await repo.GetByIdAsync(cliente.Id);

            Assert.NotNull(resultado);
            Assert.Equal("Juan", resultado.Nombre);
        }

        [Fact]
        public async Task GetByIdAsync_DeberiaRetornarNullSiNoExiste()
        {
            var repo = new ClienteRepository(CrearContextoInMemory());
            var resultado = await repo.GetByIdAsync(999);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task GetAllAsync_DeberiaRetornarSoloActivosConRolCliente()
        {
            var context = CrearContextoInMemory();
            context.Clientes.AddRange(
                CrearClienteValido("Activo1"),
                new Cliente
                {
                    Nombre = "Inactivo",
                    Apellido = "ApellidoInactivo",
                    Correo = "inactivo@test.com",
                    Contrasena = "ContrasenaInactivo",
                    Direccion = "Calle Inactivo 123",
                    Telefono = "8090000000",
                    FechaRegistro = DateTime.UtcNow,
                    Rol = "Cliente",
                    Estado = false
                },
                new Cliente
                {
                    Nombre = "Admin",
                    Apellido = "ApellidoAdmin",
                    Correo = "admin@test.com",
                    Contrasena = "ContrasenaAdmin",
                    Direccion = "Calle Admin 123",
                    Telefono = "8091111111",
                    FechaRegistro = DateTime.UtcNow,
                    Rol = "Admin",
                    Estado = true
                }
            );
            await context.SaveChangesAsync();

            var repo = new ClienteRepository(context);
            var clientes = await repo.GetAllAsync();

            Assert.Single(clientes);
            Assert.Contains(clientes, c => c.Nombre == "Activo1");
        }

        [Fact]
        public async Task UpdateAsync_DeberiaActualizarCliente()
        {
            var context = CrearContextoInMemory();
            var cliente = CrearClienteValido("Viejo");
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var repo = new ClienteRepository(context);
            cliente.Nombre = "Nuevo";
            await repo.UpdateAsync(cliente);
            await context.SaveChangesAsync();

            var actualizado = await context.Clientes.FindAsync(cliente.Id);
            Assert.Equal("Nuevo", actualizado.Nombre);
        }

        [Fact]
        public async Task DeleteAsync_DeberiaMarcarInactivo()
        {
            var context = CrearContextoInMemory();
            var cliente = CrearClienteValido("AEliminar");
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var repo = new ClienteRepository(context);
            await repo.DeleteAsync(cliente.Id);
            await context.SaveChangesAsync();

            var resultado = await context.Clientes.FindAsync(cliente.Id);
            Assert.False(resultado.Estado);
        }

        [Fact]
        public async Task DeleteAsync_NoHaceNadaSiNoExiste()
        {
            var context = CrearContextoInMemory();
            var repo = new ClienteRepository(context);

            await repo.DeleteAsync(999);
            await context.SaveChangesAsync();

            var total = await context.Clientes.CountAsync();
            Assert.Equal(0, total);
        }

        [Fact]
        public async Task GetByEmailAsync_DeberiaIgnorarMayusculas()
        {
            var context = CrearContextoInMemory();
            var cliente = CrearClienteValido("Test", "test@correo.com");
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var repo = new ClienteRepository(context);
            var resultado = await repo.GetByEmailAsync("TEST@correo.com");

            Assert.NotNull(resultado);
            Assert.Equal("test@correo.com", resultado.Correo);
        }

        [Fact]
        public async Task AddAsync_DeberiaPermitirMultiplesClientes()
        {
            var context = CrearContextoInMemory();
            var repo = new ClienteRepository(context);

            await repo.AddAsync(CrearClienteValido("A"));
            await repo.AddAsync(CrearClienteValido("B"));
            await context.SaveChangesAsync();

            var total = await context.Clientes.CountAsync();
            Assert.Equal(2, total);
        }

        [Fact]
        public async Task AddAsync_DeberiaLanzarExcepcionSiNombreEsVacio()
        {
            var context = CrearContextoInMemory();
            var repo = new ClienteRepository(context);
            var cliente = CrearClienteValido("Test");
            cliente.Nombre = "";

            await Assert.ThrowsAsync<ArgumentException>(() => repo.AddAsync(cliente));
        }

        [Fact]
        public async Task GetByEmailAsync_DeberiaRetornarNullSiEmailEsVacio()
        {
            var repo = new ClienteRepository(CrearContextoInMemory());
            var resultado = await repo.GetByEmailAsync("");

            Assert.Null(resultado);
        }
    }
}
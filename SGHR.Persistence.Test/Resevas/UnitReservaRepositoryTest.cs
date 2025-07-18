using SGHR.Persistence.Test.TestBase;
using SGHR.Domain.Entities.Reservas;
using SGHR.Persistence.Repositories.Reservas;
using SGHR.Domain.Enums;
namespace SGHR.Persistence.Test.Resevas
{
    public class UnitReservaRepositoryTest : UnitRepositoryTestBase
    {
        [Fact]
        public async Task AgregarReserva_DeberiaGuardarCorrectamente()
        {
            // Arrange
            var nuevaReserva = new Reserva(1, 2, DateTime.Now, DateTime.Today.AddDays(5), 2);
            // Act
            await ReservaRepository.CrearReservaAsync(nuevaReserva);
            await Context.SaveChangesAsync();
            var reservaGuardada = await Context.Reservas.FindAsync(nuevaReserva.Id);
            // Assert
            Assert.NotNull(reservaGuardada);
            Assert.Equal(nuevaReserva.FechaCreacion, reservaGuardada.FechaCreacion);
            Assert.Equal(nuevaReserva.ClienteId, reservaGuardada.ClienteId);
            Assert.Equal(nuevaReserva.IdCategoriaHabitacion, reservaGuardada.IdCategoriaHabitacion);
            Assert.Equal(nuevaReserva.Estado, reservaGuardada.Estado);
        }
        [Fact]
        public async Task ActualizarReserva_DeberiaActualizarCorrectamente()
        {
            // Arrange
            var reservaExistente = new Reserva(1, 2, DateTime.Now, DateTime.Today.AddDays(5), 2);
            Context.Reservas.Add(reservaExistente);
            await Context.SaveChangesAsync();
            reservaExistente.ActualizarDetalles(
                reservaExistente.ClienteId,
                reservaExistente.IdCategoriaHabitacion,
                reservaExistente.FechaEntrada,
                DateTime.Today.AddDays(10),
                reservaExistente.NumeroHuespedes);

            // Act
            await ReservaRepository.ActualizarReservaAsync(reservaExistente);
            await Context.SaveChangesAsync();
            // Assert
            var reservaActualizada = await Context.Reservas.FindAsync(reservaExistente.Id);
            Assert.NotNull(reservaActualizada);
            Assert.Equal(DateTime.Today.AddDays(10), reservaActualizada.FechaSalida);
        }
        [Fact]
        public async Task CancelarReserva_DeberiaCancelarCorrectamente()
        {
            // Arrange
            var reservaExistente = new Reserva(1, 2, DateTime.Now, DateTime.Today.AddDays(5), 1);
            reservaExistente.Confirmar();
            Context.Reservas.Add(reservaExistente);
            await Context.SaveChangesAsync();
            // Act
            reservaExistente.Cancelar();
            await ReservaRepository.ActualizarReservaAsync(reservaExistente);
            await Context.SaveChangesAsync();
            // Assert
            var reservaCancelada = await Context.Reservas.FindAsync(reservaExistente.Id);
            Assert.NotNull(reservaCancelada);
            Assert.Equal(EstadoReserva.Cancelada, reservaCancelada.Estado);
        }
        [Fact]
        public async Task ConfirmarReserva_DeberiaConfirmarCorrectamente()
        {
            // Arrange
            var reservaExistente = new Reserva(1, 2, DateTime.Now, DateTime.Today.AddDays(5), 1);
            Context.Reservas.Add(reservaExistente);
            await Context.SaveChangesAsync();
            // Act
            reservaExistente.Confirmar();
            await ReservaRepository.ActualizarReservaAsync(reservaExistente);
            await Context.SaveChangesAsync();
            // Assert
            var reservaConfirmada = await Context.Reservas.FindAsync(reservaExistente.Id);
            Assert.NotNull(reservaConfirmada);
            Assert.Equal(EstadoReserva.Confirmada, reservaConfirmada.Estado);
        }
        // se supone que la base de datos tiene un procedimiento almacenado llamado "HayDisponibilidadHabitacion" pero al estar en memoria no se puede ejecutar
        [Fact]
        public async Task HayDisponibilidad_DeberiaRetornarTrueSiHayDisponibilidad()
        {
            // Arrange
            var reservaExistente = new Reserva(1, 2, DateTime.Now, DateTime.Today.AddDays(5), 1);
            Context.Reservas.Add(reservaExistente);
            await Context.SaveChangesAsync();
            // Act
            var disponibilidad = await ReservaRepository.HayDisponibilidadAsync(reservaExistente.IdCategoriaHabitacion, DateTime.Now, DateTime.Today.AddDays(3));
            // Assert
            Assert.True(disponibilidad);
        }
        [Fact]
        public async Task ObtenerPorId_DeberiaRetornarReserva()
        {
            var reserva = new Reserva(1, 2, DateTime.Now, DateTime.Today.AddDays(5), 2);
            Context.Reservas.Add(reserva);
            await Context.SaveChangesAsync();
            // Act
            var reservaObtenida = await ReservaRepository.ObtenerPorId(reserva.Id);
            // Assert
            Assert.NotNull(reservaObtenida);
            Assert.Equal(reserva.Id, reservaObtenida.Id);
            Assert.Equal(reserva.ClienteId, reservaObtenida.ClienteId);
            Assert.Equal(reserva.IdCategoriaHabitacion, reservaObtenida.IdCategoriaHabitacion);
            Assert.Equal(reserva.FechaCreacion, reservaObtenida.FechaCreacion);
            Assert.Equal(reserva.Estado, reservaObtenida.Estado);
        }
        [Fact]
        public async Task ObtenerTodas_DeberiaRetornarTodasLasReservas()
        {
            // Arrange
            var reserva1 = new Reserva(1, 2, DateTime.Now, DateTime.Today.AddDays(5), 2);
            var reserva2 = new Reserva(3, 4, DateTime.Now.AddDays(1), DateTime.Today.AddDays(6), 3);
            Context.Reservas.Add(reserva1);
            Context.Reservas.Add(reserva2);
            await Context.SaveChangesAsync();

            // Act
            var reservas = await ReservaRepository.ObtenerTodasAsync();
            // Assert
            Assert.NotNull(reservas);
            Assert.Equal(2, reservas.Count);
            Assert.Contains(reservas, r => r.Id == reserva1.Id);
            Assert.Contains(reservas, r => r.Id == reserva2.Id);
        }
        [Fact]
        public async Task ObtenerReservasPorCliente_DeberiaRetornarReservasDelCliente()
        {
            // Arrange
            var reserva1 = new Reserva(1, 2, DateTime.Now, DateTime.Today.AddDays(5), 2);
            var reserva2 = new Reserva(1, 3, DateTime.Now.AddDays(1), DateTime.Today.AddDays(6), 3);
            Context.Reservas.Add(reserva1);
            Context.Reservas.Add(reserva2);
            await Context.SaveChangesAsync();
            // Act
            var reservasCliente = await ReservaRepository.ObtenerPorClienteIdAsync(1);
            // Assert
            Assert.NotNull(reservasCliente);
            Assert.Equal(2, reservasCliente.Count);
            Assert.Contains(reservasCliente, r => r.Id == reserva1.Id);
            Assert.Contains(reservasCliente, r => r.Id == reserva2.Id);
        }
        [Fact]
        public async Task ObtenerReservasEnRango_DeberiaRetornarReservasEnRango()
        {
            // Arrange
            var reserva1 = new Reserva(1, 2, DateTime.Now, DateTime.Today.AddDays(5), 2);
            var reserva2 = new Reserva(3, 4, DateTime.Now.AddDays(1), DateTime.Today.AddDays(6), 3);
            Context.Reservas.Add(reserva1);
            Context.Reservas.Add(reserva2);
            await Context.SaveChangesAsync();
            // Act
            var reservasRango = await ReservaRepository.ObtenerReservasEnRangoAsync(DateTime.Now, DateTime.Today.AddDays(7));
            // Assert
            Assert.NotNull(reservasRango);
            Assert.Equal(2, reservasRango.Count);
            Assert.Contains(reservasRango, r => r.Id == reserva1.Id);
            Assert.Contains(reservasRango, r => r.Id == reserva2.Id);
        }
    }
}

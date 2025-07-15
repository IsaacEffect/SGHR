using Moq;
using AutoMapper;
using SGHR.Application.DTOs.Reservas;
using SGHR.Application.Services.Reservas;
using SGHR.Persistence.Interfaces.Repositories.Reservas;
using SGHR.Persistence.Interfaces.Repositories.Clientes;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Users;
using SGHR.Domain.Entities.Reservas;
using SGHR.Domain.Interfaces;
using SGHR.Domain.Enums;

namespace SGHR.Application.Test.Reservas
{
    public class ReservaApplicationServiceTest
    {

        private readonly Mock<ICategoriaHabitacionRepository> _categoriaHabitacionRepMock = new();
        private readonly Mock<IClienteRepository> _clienteRepMock = new();
        private readonly Mock<IReservaRepository> _reservaRepMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IReservaRules> _reservaRulesMock = new();

        private readonly ReservaApplicationService _service;
        public ReservaApplicationServiceTest()
        {
            _service = new ReservaApplicationService(
                _reservaRepMock.Object,
                _categoriaHabitacionRepMock.Object,
                _clienteRepMock.Object,
                _mapperMock.Object,
                _unitOfWorkMock.Object,
                _reservaRulesMock.Object);
        }



        [Fact]
        public async Task CrearReservaAsync_CreaReservaCorrectamente()
        {
            var request = new CrearReservaRequest
            {
                ClienteId = 1,
                IdCategoriaHabitacion = 1,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(2),
                NumeroHuespedes = 2
            };
           

            var clienteMock = new Cliente(
                nombre: "Test Cliente",
                hashedPassword: "hashed",
                email: "cliente@test.com",
                rol: RolUsuario.Cliente,
                apellido: "Apellido"
            );

            _mapperMock.Setup(m => m.Map<Reserva>(It.IsAny<CrearReservaRequest>()))
                .Returns(new Reserva(
                    request.ClienteId,
                    request.IdCategoriaHabitacion,
                    request.FechaEntrada,
                    request.FechaSalida,
                    request.NumeroHuespedes
                ));
            _clienteRepMock.Setup(c => c.ObtenerPorId(request.ClienteId))
                           .ReturnsAsync(clienteMock);

            _reservaRulesMock.Setup(r => r.ValidarExistenciaClienteAsync(request.ClienteId)).Returns(Task.CompletedTask);
            _reservaRulesMock.Setup(r => r.ValidarFechaEntradaMayorSalida(request.FechaEntrada, request.FechaSalida)).Returns(Task.CompletedTask);

            _categoriaHabitacionRepMock.Setup(c =>c.HayDisponibilidadAsync(
                    request.IdCategoriaHabitacion,
                    request.FechaEntrada,
                    request.FechaSalida,
                    null
            )).ReturnsAsync(true);
            
            _reservaRulesMock.Setup(r => r.ValidarExistenciaCategoriaAsync(request.IdCategoriaHabitacion, true)).Returns(Task.CompletedTask);

           
            _mapperMock.Setup(m => m.Map<ReservaDto>(It.IsAny<Reserva>()))
                .Returns(new ReservaDto
                {
                    Id = 1,
                    IdCliente = request.ClienteId,
                    IdCategoriaHabitacion = request.IdCategoriaHabitacion,
                    FechaEntrada = request.FechaEntrada,
                    FechaSalida = request.FechaSalida,
                    Estado = EstadoReserva.Pendiente,
                    NumeroHuespedes = request.NumeroHuespedes
                });

            _reservaRepMock.Setup(r => r.CrearReservaAsync(It.IsAny<Reserva>()))
                .Returns(Task.FromResult(request));
            var resultado = await _service.CrearReservaAsync(request);
            Assert.NotNull(resultado);
            Assert.Equal(1,resultado.Id);
        }

        [Fact]
        public async Task CrearReservaAsync_LanzaExcepcion_SiNoHayDisponibilidad()
        {
            var request = new CrearReservaRequest
            {
                ClienteId = 1,
                IdCategoriaHabitacion = 1,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(1),
                NumeroHuespedes = 1
            };

            _reservaRulesMock.Setup(r => r.ValidarExistenciaClienteAsync(request.ClienteId)).Returns(Task.CompletedTask);
            _reservaRulesMock.Setup(r => r.ValidarFechaEntradaMayorSalida(request.FechaEntrada, request.FechaSalida)).Returns(Task.CompletedTask);

            _categoriaHabitacionRepMock.Setup(c =>
                c.HayDisponibilidadAsync(
                    request.IdCategoriaHabitacion,
                    request.FechaEntrada,
                    request.FechaSalida,
                    null
                )
            ).ReturnsAsync(false);

            _reservaRulesMock.Setup(r =>
                r.ValidarExistenciaCategoriaAsync(request.IdCategoriaHabitacion, false)
            ).ThrowsAsync(new ArgumentException("No hay disponibilidad para la categoría de habitación 1 en el rango de fechas especificado para la actualización."));

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CrearReservaAsync(request));

            Assert.Contains("No hay disponibilidad", ex.Message);
        }
        [Fact]
        public async Task CrearReservaAsync_LanzaExcepcion_SiClienteNoExiste()
        {
            var request = new CrearReservaRequest
            {
                ClienteId = 1,
                IdCategoriaHabitacion = 1,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(1),
                NumeroHuespedes = 1
            };
            _reservaRulesMock.Setup(r => r.ValidarExistenciaClienteAsync(request.ClienteId))
                .ThrowsAsync(new ArgumentException("El cliente con ID 1 no existe."));
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.CrearReservaAsync(request));
            Assert.Contains("El cliente con ID 1 no existe.", ex.Message);
        }
        [Fact]
        public async Task CancelarReservaAsync_NoDebePermitirCancelarFinalizada()
        {
            var reserva = new Reserva(1, 1, DateTime.Today, DateTime.Today.AddDays(5), 2);
            reserva.Confirmar();
            reserva.Finalizar();
            _reservaRepMock.Setup(r => r.ObtenerPorId(It.IsAny<int>())).ReturnsAsync(reserva);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(()=> _service.CancelarReservaAsync(1));
            Assert.Contains("No se puede cancelar una reserva que ya", ex.Message);
        }
        [Fact]
        public async Task ActualizarReservaAsync_NoDebePermitirFechasInvalidas()
        {
            
            var reserva = new Reserva(1, 1, DateTime.Today, DateTime.Today.AddDays(5), 2);
            _reservaRepMock.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(reserva);

            var request = new ActualizarReservaRequest 
            { 
                IdCliente = 1,
                IdCategoriaHabitacion = 1,
                FechaEntrada = DateTime.Today.AddDays(6),
                FechaSalida = DateTime.Today.AddDays(4),
                NumeroHuespedes = 2,
                Estado = EstadoReserva.Pendiente 
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.ActualizarReservaAsync(1, request));
            Assert.Contains("La fecha de entrada no puede ser posterior", ex.Message);
        }
        [Fact]
        public async Task VerificarDisponibilidadAsync_DevuelveFalseSiNoExisteCategoria()
        {
            var request = new VerificarDisponibilidadRequest
            {
                IdCategoriaHabitacion = 1,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(5)
            };

            _reservaRulesMock
                .Setup(r => r.ValidarExistenciaCategoriaAsync(request.IdCategoriaHabitacion, true))
                .ThrowsAsync(new ArgumentException("La categoria con ID 1 no existe."));

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.VerificarDisponibilidadAsync(request));
            Assert.Contains("La categoria con ID 1 no existe.", ex.Message);
        }
    }
}

using AutoMapper;
using Moq;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Services.Servicios;
using SGHR.Domain.Entities.Habitaciones;
using SGHR.Domain.Interfaces;
using ServicioE = SGHR.Domain.Entities.Servicios.Servicios;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Servicios;

namespace SGHR.Application.Test.Servicios
{
    public class ServicioCategoriaApplicationServiceTest
    {
        private readonly Mock<IServicioCategoriaRepository> _servicioCategoriaRepMock = new();
        private readonly Mock<IServicioRepository> _servicioRepMock = new();
        private readonly Mock<ICategoriaHabitacionRepository> _catHabitacionRepMock = new();
        private readonly Mock<IServicioRules> _servicioRulesMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

        private readonly IServicioCategoriaApplicationService _service;
        public ServicioCategoriaApplicationServiceTest()
        {
            _service = new ServicioCategoriaApplicationService(
                _servicioCategoriaRepMock.Object,
                _servicioRepMock.Object,
                _catHabitacionRepMock.Object,
                _servicioRulesMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task AsignarPrecioServicioCategoriaAsync_DebeAsignarPrecioCorrectamente()
        {
            // Arrange
            var request = new AsignarPrecioServicioCategoriaRequest
            {
                IdServicio = 1,
                IdCategoriaHabitacion = 1,
                Precio = 200.00m
            };
            var servicioEntity = new ServicioE("Servicio de Lavandería", "Lavado y planchado de ropa");
            var categoriaHabitacionEntity = new CategoriaHabitacion();

            _catHabitacionRepMock.Setup(r => r.ObtenerPorIdAsync(request.IdCategoriaHabitacion)).ReturnsAsync(categoriaHabitacionEntity);
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(request.IdServicio)).Returns(Task.CompletedTask);
            _servicioRepMock.Setup(r => r.ObtenerPorIdAsync(request.IdServicio)).ReturnsAsync(servicioEntity);
            
            _servicioCategoriaRepMock.Setup(r => r.AgregarPrecioServicioCategoriaAsync(request.IdServicio, request.IdCategoriaHabitacion, request.Precio)).Returns(Task.CompletedTask);
            // Act
            await _service.AsignarPrecioServicioCategoriaAsync(request);
            // Assert
            _servicioCategoriaRepMock.Verify(r => r.AgregarPrecioServicioCategoriaAsync(request.IdServicio, request.IdCategoriaHabitacion, request.Precio), Times.Once);
        }
        [Fact]
        public async Task AsignarPrecioServicioCategoriaAsync_LanzaExcepcion_SiCategoriaNoExiste()
        {
            // Arrange
            var request = new AsignarPrecioServicioCategoriaRequest
            {
                IdServicio = 1,
                IdCategoriaHabitacion = 999,
                Precio = 200.00m
            };
            _catHabitacionRepMock.Setup(r => r.ObtenerPorIdAsync(request.IdCategoriaHabitacion)).ReturnsAsync((CategoriaHabitacion)null);
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AsignarPrecioServicioCategoriaAsync(request));
        }
        [Fact]
        public async Task AsignarPrecioServicioCategoriaAsync_LanzaExcepcion_SiDatosInvalidos()
        {
            // Arrange
            var request = new AsignarPrecioServicioCategoriaRequest
            {
                IdServicio = 1,
                IdCategoriaHabitacion = 1,
                Precio = -100.00m
            };
            _servicioRulesMock.Setup(r => r.ValidarPrecioServicio(request.Precio))
                .Throws(new ArgumentException("El precio del servicio no puede ser negativo."));
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AsignarPrecioServicioCategoriaAsync(request));
        }
        [Fact]
        public async Task ActualizarPrecioServicioCategoriaAsync_DebeActualizarPrecioCorrectamente()
        {
            // Arrange
            var request = new ActualizarPrecioServicioCategoriaRequest
            {
                IdServicio = 1,
                IdCategoriaHabitacion = 1,
                Precio = 250.00m
            };
            var servicioEntity = new ServicioE("Servicio de Lavandería", "Lavado y planchado de ropa");
            var categoriaHabitacionEntity = new CategoriaHabitacion();
            _catHabitacionRepMock.Setup(r => r.ObtenerPorIdAsync(request.IdCategoriaHabitacion)).ReturnsAsync(categoriaHabitacionEntity);
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(request.IdServicio)).Returns(Task.CompletedTask);
            _servicioRepMock.Setup(r => r.ObtenerPorIdAsync(request.IdServicio)).ReturnsAsync(servicioEntity);
            _servicioCategoriaRepMock.Setup(r => r.ActualizarPrecioServicioCategoriaAsync(request.IdServicio, request.IdCategoriaHabitacion, request.Precio)).Returns(Task.CompletedTask);
            // Act
            await _service.ActualizarPrecioServicioCategoriaAsync(request);
            // Assert
            _servicioCategoriaRepMock.Verify(r => r.ActualizarPrecioServicioCategoriaAsync(request.IdServicio, request.IdCategoriaHabitacion, request.Precio), Times.Once);
        }
        [Fact]
        public async Task ActualizarPrecioServicioCategoriaAsync_LanzaExcepcion_SiDatosInvalidos()
        {
            // Arrange
            var request = new ActualizarPrecioServicioCategoriaRequest
            {
                IdServicio = 1,
                IdCategoriaHabitacion = 1,
                Precio = -150.00m
            };
            _servicioRulesMock.Setup(r => r.ValidarPrecioServicio(request.Precio))
                .Throws(new ArgumentException("El precio del servicio no puede ser negativo."));
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.ActualizarPrecioServicioCategoriaAsync(request));
        }
        [Fact]
        public async Task EliminarPrecioServicioCategoriaAsync_DebeEliminarPrecioCorrectamente()
        {
            // Arrange
            var request = new AsignarPrecioServicioCategoriaRequest
            {
                IdServicio = 1,
                IdCategoriaHabitacion = 1,
                Precio = 200.00m
            };
            _servicioRulesMock.Setup(r => r.ValidarIdPositivo(request.IdServicio));
            _servicioRulesMock.Setup(r => r.ValidarIdPositivo(request.IdCategoriaHabitacion));
            _servicioCategoriaRepMock.Setup(r => r.EliminarPrecioServicioCategoriaAsync(request.IdServicio, request.IdCategoriaHabitacion)).Returns(Task.CompletedTask);
            _catHabitacionRepMock.Setup(r => r.ObtenerPorIdAsync(request.IdCategoriaHabitacion))
            .ReturnsAsync(new CategoriaHabitacion());

            // Act
            await _service.EliminarPrecioServicioCategoriaAsync(request.IdServicio, request.IdCategoriaHabitacion);
            // Assert
            _servicioCategoriaRepMock.Verify(r =>r.EliminarPrecioServicioCategoriaAsync(request.IdServicio, request.IdCategoriaHabitacion),Times.Once);
        }
        [Fact]
        public async Task EliminarPrecioServicioCategoriaAsync_LanzaExcepcion_SiDatosInvalidos()
        {
            // Arrange
            int servicioId = -1;
            int categoriaId = -1;
            _servicioRulesMock.Setup(r => r.ValidarIdPositivo(servicioId))
                .Throws(new ArgumentException("El ID del servicio debe ser positivo."));
            _servicioRulesMock.Setup(r => r.ValidarIdPositivo(categoriaId))
                .Throws(new ArgumentException("El ID de la categoría de habitación debe ser positivo."));
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.EliminarPrecioServicioCategoriaAsync(servicioId, categoriaId));
        }
        

    }
}

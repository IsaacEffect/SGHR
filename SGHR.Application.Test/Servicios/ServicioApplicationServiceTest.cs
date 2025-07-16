using AutoMapper;
using Moq;
using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Application.Services.Servicios;
using SGHR.Domain.Entities.Habitaciones;
using SGHR.Domain.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using ServiciosE = SGHR.Domain.Entities.Servicios.Servicios;

namespace SGHR.Application.Test.Servicios
{
    public class ServicioApplicationServiceTest 
    {
        private readonly Mock<IServicioRepository> _servicioRepMock = new();
        private readonly Mock<IServicioCategoriaRepository> _servicioCategoriaRepMock = new();
        private readonly Mock<ICategoriaHabitacionRepository> _catHabitacionRepMock = new();
        private readonly Mock<IServicioRules> _servicioRulesMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IUnitOfWork> _unit0fWorkMock = new();

        private readonly IServicioApplicationService _service;
        public ServicioApplicationServiceTest()
        {
            _service = new ServicioApplicationService(
                _servicioRepMock.Object,
                _servicioCategoriaRepMock.Object,
                _catHabitacionRepMock.Object,
                _mapperMock.Object,
                _unit0fWorkMock.Object,
                _servicioRulesMock.Object

            );

        }
        [Fact]
        public async Task AgregarServicioAsync_DebeCrearServicioCorrectamente()
        {
            var request = new AgregarServicioRequest
            {
                Nombre = "Servicio de Limpieza",
                Descripcion = "Limpieza diaria de habitaciones",
                Activo = true
            };

            var servicioEntity = new ServiciosE("Servicio de Limpieza", "Limpieza diaria de habitaciones");
            servicioEntity.Activar();

            _mapperMock.Setup(m => m.Map<ServiciosE>(request)).Returns(servicioEntity);
            _servicioRepMock.Setup(r => r.AgregarServicioAsync(servicioEntity)).Returns(Task.CompletedTask);
            _unit0fWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            _mapperMock.Setup(m => m.Map<ServicioDto>(servicioEntity))
                .Returns(new ServicioDto
                {
                    Nombre = servicioEntity.Nombre,
                    Descripcion = servicioEntity.Descripcion,
                    Activo = servicioEntity.Activo
                });

            var resultado = await _service.AgregarServicioAsync(request);
            Assert.NotNull(resultado);
            Assert.Equal("Servicio de Limpieza", resultado.Nombre);
            Assert.True(resultado.Activo);

        }
        [Fact]
        public async Task AgregarServicioAsync_LanzaExcepcion_SiDatosInvalidos()
        {
            var request = new AgregarServicioRequest
            {
                Nombre = "",
                Descripcion = "Limpieza diaria de habitaciones",
                Activo = true
            };
            _servicioRulesMock.Setup(r => r.ValidarDatosBasicosAsync(request.Nombre, request.Descripcion))
                .ThrowsAsync(new ArgumentException("El nombre del servicio no puede estar vacío."));
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AgregarServicioAsync(request));
        }
        [Fact]
        public async Task ActualizarServicioAsync_DebeActualizarServicioCorrectamente()
        {
            var request = new ActualizarServicioRequest
            {
                IdServicio = 1,
                Nombre = "Servicio de Lavandería",
                Descripcion = "Lavado y planchado de ropa",
                Activo = true
            };

            var servicioEntity = new ServiciosE("Servicio de Lavandería", "Lavado y planchado de ropa");
            servicioEntity.Activar();

            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(request.IdServicio)).Returns(Task.CompletedTask);
            _servicioRepMock.Setup(r => r.ObtenerPorIdAsync(request.IdServicio)).ReturnsAsync(servicioEntity);
            _servicioRulesMock.Setup(r => r.ValidarDatosBasicosAsync(request.Nombre, request.Descripcion)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ServiciosE>(request)).Returns(servicioEntity);
            _servicioRepMock.Setup(r => r.ActualizarServicioAsync(servicioEntity)).Returns(Task.CompletedTask);
            _unit0fWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            await _service.ActualizarServicioAsync(request);
            Assert.Equal("Servicio de Lavandería", servicioEntity.Nombre);
            Assert.Equal("Lavado y planchado de ropa", servicioEntity.Descripcion);
            Assert.True(servicioEntity.Activo);
        }
        [Fact]
        public async Task ActualizarServicioAsync_LanzaExcepcion_SiServicioNoExiste()
        {
            var request = new ActualizarServicioRequest
            {
                IdServicio = 1,
                Nombre = "Servicio de Lavandería",
                Descripcion = "Lavado y planchado de ropa",
                Activo = true
            };
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(request.IdServicio))
                .ThrowsAsync(new KeyNotFoundException($"El servicio con ID {request.IdServicio} no fue encontrado"));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ActualizarServicioAsync(request));
        }
        [Fact]
        public async Task EliminarServicioAsync_DebeEliminarServicioCorrectamente()
        {
            int idServicio = 1;
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(idServicio)).Returns(Task.CompletedTask);
            _servicioRepMock.Setup(r => r.ObtenerPorIdAsync(idServicio)).ReturnsAsync(new ServiciosE("Servicio de Lavandería", "Lavado y planchado de ropa"));
            _servicioRepMock.Setup(r => r.EliminarServicioAsync(idServicio)).Returns(Task.CompletedTask);
            _unit0fWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            await _service.EliminarServicioAsync(idServicio);
            _servicioRepMock.Verify(r => r.EliminarServicioAsync(idServicio), Times.Once);
        }
        [Fact]
        public async Task EliminarServicioAsync_LanzaExcepcion_SiServicioNoExiste()
        {
            int idServicio = 1;
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(idServicio))
                .ThrowsAsync(new KeyNotFoundException($"El servicio con ID {idServicio} no fue encontrado"));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.EliminarServicioAsync(idServicio));
        }

        [Fact]
        public async Task ActivarServicioAsync_DebeActivarServicioCorrectamente()
        {
            int idServicio = 1;
            var servicioEntity = new ServiciosE("Servicio de Lavandería", "Lavado y planchado de ropa");
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(idServicio)).Returns(Task.CompletedTask);
            _servicioRepMock.Setup(r => r.ObtenerPorIdAsync(idServicio)).ReturnsAsync(servicioEntity);

            servicioEntity.Activar();
            _servicioRepMock.Setup(r => r.ActualizarServicioAsync(servicioEntity)).Returns(Task.CompletedTask);
            _unit0fWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            await _service.ActivarServicioAsync(idServicio);
            Assert.True(servicioEntity.Activo);
        }
        [Fact]
        public async Task ActivarServicioAsync_LanzaExcepcion_SiServicioNoExiste()
        {
            int idServicio = 1;
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(idServicio))
                .ThrowsAsync(new KeyNotFoundException($"El servicio con ID {idServicio} no fue encontrado"));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ActivarServicioAsync(idServicio));
        }
        [Fact]
        public async Task DesactivarServicioAsync_DebeDesactivarServicioCorrectamente()
        {
            int idServicio = 1;
            var servicioEntity = new ServiciosE("Servicio de Lavandería", "Lavado y planchado de ropa");
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(idServicio)).Returns(Task.CompletedTask);
            _servicioRepMock.Setup(r => r.ObtenerPorIdAsync(idServicio)).ReturnsAsync(servicioEntity);

            servicioEntity.Desactivar();
            _servicioRepMock.Setup(r => r.ActualizarServicioAsync(servicioEntity)).Returns(Task.CompletedTask);
            _unit0fWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            await _service.DesactivarServicioAsync(idServicio);
            Assert.False(servicioEntity.Activo);
        }
        [Fact]
        public async Task DesactivarServicioAsync_LanzaExcepcion_SiServicioNoExiste()
        {
            int idServicio = 1;
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(idServicio))
                .ThrowsAsync(new KeyNotFoundException($"El servicio con ID {idServicio} no fue encontrado"));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DesactivarServicioAsync(idServicio));
        }

        [Fact]
        public async Task AsignarPrecioServicioCategoriaAsync_DebeAsignarPrecioCorrectamente()
        {
  
            var request = new AsignarPrecioServicioCategoriaRequest
            {
                IdServicio = 1,
                IdCategoriaHabitacion = 1,
                Precio = 200.00m
            };
            var servicioEntity = new ServiciosE("Servicio de Lavandería", "Lavado y planchado de ropa");
            var categoriaHabitacionEntity = new CategoriaHabitacion();

            _catHabitacionRepMock.Setup(r => r.ObtenerPorIdAsync(request.IdCategoriaHabitacion)).ReturnsAsync(categoriaHabitacionEntity);
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(request.IdServicio)).Returns(Task.CompletedTask);
            _servicioRepMock.Setup(r => r.ObtenerPorIdAsync(request.IdServicio)).ReturnsAsync(servicioEntity);
    
            _servicioCategoriaRepMock.Setup(r => r.AgregarPrecioServicioCategoriaAsync(request.IdServicio, request.IdCategoriaHabitacion, request.Precio)).Returns(Task.CompletedTask);

            await _service.AsignarPrecioServicioCategoriaAsync(request);
        }

        [Fact]
        public async Task AsignarPrecioServicioCategoriaAsync_LanzaExcepcion_SiDatosInvalidos()
        {
            var request = new AsignarPrecioServicioCategoriaRequest
            {
                IdServicio = 1,
                IdCategoriaHabitacion = 1,
                Precio = -100.00m 
            };
            _servicioRulesMock.Setup(r => r.ValidarPrecioServicio(request.Precio))
                .Throws(new ArgumentException("El precio del servicio no puede ser negativo."));
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AsignarPrecioServicioCategoriaAsync(request));
        }

        [Fact]
        public async Task ObtenerServicioPorId_Async_DebeRetornarServicioCorrectamente()
        {
            int idServicio = 1;
            var servicioEntity = new ServiciosE("Servicio de Lavandería", "Lavado y planchado de ropa");
            _mapperMock.Setup(m => m.Map<ServicioDto>(servicioEntity))
                .Returns(new ServicioDto
                {
                    Nombre = servicioEntity.Nombre,
                    Descripcion = servicioEntity.Descripcion,
                    Activo = servicioEntity.Activo
                });
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(idServicio)).Returns(Task.CompletedTask);
            _servicioRepMock.Setup(r => r.ObtenerPorIdAsync(idServicio)).ReturnsAsync(servicioEntity);

            var resultado = await _service.ObtenerServicioPorIdAsync(idServicio);
            Assert.NotNull(resultado);
            Assert.Equal("Servicio de Lavandería", resultado.Nombre);
        }
        [Fact]
        public async Task ObtenerServicioPorId_Async_LanzaExcepcion_SiServicioNoExiste()
        {
            int idServicio = 1;
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(idServicio))
                .ThrowsAsync(new KeyNotFoundException($"El servicio con ID {idServicio} no fue encontrado"));
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ObtenerServicioPorIdAsync(idServicio));
        }
    }
}

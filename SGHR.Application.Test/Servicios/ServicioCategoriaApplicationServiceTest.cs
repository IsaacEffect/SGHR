using AutoMapper;
using Azure.Core;
using Moq;
using Moq.Language.Flow;
using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Application.Services.Servicios;
using SGHR.Domain.Entities.Habitaciones;
using SGHR.Domain.Enums;
using SGHR.Domain.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using ServicioCatE = SGHR.Domain.Entities.Servicios.ServicioCategoria;
using ServicioE = SGHR.Domain.Entities.Servicios.Servicios;

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
            var categoriaHabitacionEntity = new CategoriaHabitacion(request.IdCategoriaHabitacion, "suite", "descripcion", 1000.00m, "caracteristicas");

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
            var categoriaHabitacionEntity = new CategoriaHabitacion(request.IdCategoriaHabitacion, "suite", "descripcion", 1000.00m, "caracteristicas");
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
            .ReturnsAsync(new CategoriaHabitacion(request.IdCategoriaHabitacion, "suite","descripcion",1000.00m,"caracteristicas"));

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
        [Fact]
        public async Task ObtenerPreciosServicioPorCategoriaAsync_DebeRetornarPreciosCorrectamente()
        {
            // Arrange
            var categoriaId = 1;
            var precios = new List<ServicioCatE>
            {
                new ( 1, categoriaId, 100.00m ),
                new (2, categoriaId, 150.00m )
            };
            _catHabitacionRepMock.Setup(r => r.ObtenerPorIdAsync(categoriaId))
                .ReturnsAsync(new CategoriaHabitacion(
                    categoriaId,
                    "Suite",
                    "Suite con vista al mar",
                    4000m,
                    "caracteristicas"
                ));
            _servicioCategoriaRepMock.Setup(r => r.ObtenerPreciosPorCategoriaAsync(categoriaId)).ReturnsAsync(precios);
            _mapperMock.Setup(m => m.Map<List<ServicioCategoriaDto>>(precios))
                .Returns(precios.Select(p => new ServicioCategoriaDto
                {
                    IdServicio = p.IdServicio,
                    IdCategoriaHabitacion = p.IdCategoriaHabitacion,
                    Precio = p.Precio
                }).ToList());
            // Act
            var result = await _service.ObtenerPreciosServicioPorCategoriaAsync(categoriaId);
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(100.00m, result[0].Precio);
            Assert.Equal(150.00m, result[1].Precio);
        }
        [Fact]
        public async Task ObtenerPreciosServicioPorCategoriaAsync_LanzaExcepcion_SiCategoriaNoExiste()
        {
            // Arrange
            int categoriaId = 999;

            _servicioCategoriaRepMock.Setup(r => r.ObtenerPreciosPorCategoriaAsync(categoriaId)).ReturnsAsync((List<ServicioCatE>)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ObtenerPreciosServicioPorCategoriaAsync(categoriaId));
        }
        [Fact]
        public async Task ObtenerPreciosCategoriaPorServicioAsync_DebeRetornarPreciosCorrectamente()
        {
            // Arrange
            var servicioId = 1;
            var precios = new List<ServicioCatE>
            {
                new (servicioId, 1, 100.00m),
                new (servicioId, 2, 150.00m)
            };
            _servicioRepMock.Setup(r => r.ObtenerPorIdAsync(servicioId))
                .ReturnsAsync(new ServicioE("Servicio de Lavandería", "Lavado y planchado de ropa"));
            _servicioCategoriaRepMock.Setup(r => r.ObtenerPreciosPorServicioAsync(servicioId)).ReturnsAsync(precios);
            _mapperMock.Setup(m => m.Map<List<ServicioCategoriaDto>>(precios))
                .Returns(precios.Select(p => new ServicioCategoriaDto
                {
                    IdServicio = p.IdServicio,
                    IdCategoriaHabitacion = p.IdCategoriaHabitacion,
                    Precio = p.Precio
                }).ToList());
            // Act
            var result = await _service.ObtenerPreciosCategoriaPorServicioAsync(servicioId);
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(100.00m, result[0].Precio);
            Assert.Equal(150.00m, result[1].Precio);
        }
        [Fact]
        public async Task ObtenerPreciosCategoriaPorServicioAsync_LanzaExcepcion_SiServicioNoExiste()
        {
            // Arrange
            int servicioId = 999;
            _servicioRulesMock.Setup(r => r.ValidarExistenciaSerivicioAsync(servicioId))
                .Throws(new KeyNotFoundException($"El servicio con el {servicioId} no existe"));
            _servicioRepMock.Setup(r => r.ObtenerPorIdAsync(servicioId)).ReturnsAsync((ServicioE)null);
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ObtenerPreciosCategoriaPorServicioAsync(servicioId));
        }
        [Fact]
        public async Task ObtenerPrecioServicioCategoriaEspecificoAsync_DebeRetornarPrecioCorrectamente()
        {
            // Arrange
            int servicioId = 1;
            int categoriaId = 1;
            var precio = new ServicioCatE(servicioId, categoriaId, 200.00m);

            _catHabitacionRepMock.Setup(r => r.ObtenerPorIdAsync(categoriaId))
                .ReturnsAsync(new CategoriaHabitacion(categoriaId, "Suite", "Suite con vista al mar", 4000m, "caracteristicas"));

            _servicioCategoriaRepMock.Setup(r => r.ObtenerPrecioServicioCategoriaEspecificoAsync(servicioId, categoriaId)).ReturnsAsync(precio);
            _mapperMock.Setup(m => m.Map<ServicioCategoriaDto>(precio))
                .Returns(new ServicioCategoriaDto
                {
                    IdServicio = precio.IdServicio,
                    IdCategoriaHabitacion = precio.IdCategoriaHabitacion,
                    Precio = precio.Precio
                });
            // Act
            var result = await _service.ObtenerPrecioServicioCategoriaEspecificoAsync(servicioId, categoriaId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(servicioId, result.IdServicio);
            Assert.Equal(categoriaId, result.IdCategoriaHabitacion);
            Assert.Equal(200.00m, result.Precio);
        }
        [Fact]
        public async Task ObtenerPrecioServicioCategoriaEspecificoAsync_LanzaExcepcion_SiDatosInvalidos()
        {
            // Arrange
            int servicioId = -1;
            int categoriaId = -1;
            _servicioRulesMock.Setup(r => r.ValidarIdPositivo(servicioId))
                .Throws(new ArgumentException("El ID del servicio debe ser positivo."));
            _servicioRulesMock.Setup(r => r.ValidarIdPositivo(categoriaId))
                .Throws(new ArgumentException("El ID de la categoría de habitación debe ser positivo."));
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.ObtenerPrecioServicioCategoriaEspecificoAsync(servicioId, categoriaId));
        }

    }
}

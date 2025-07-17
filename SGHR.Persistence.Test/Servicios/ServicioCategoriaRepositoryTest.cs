using SGHR.Persistence.Repositories.Servicios;
using SGHR.Persistence.Test.TestBase;
namespace SGHR.Persistence.Test.Servicios
{
    public class ServicioCategoriaRepositoryTest : RepositoryTestBase
    {
        // "Funciona" solo que para pruebas unitarias no se puede usar SQL Server, o por lo menos me tira el excepction 
        [Fact]
        public async Task AgregarPrecioServicioCategoriaAsync_DeberiaAgregarCorrectamente()
        {
            // Arrange
            
            int servicioId = 1;
            int categoriaId = 2;
            decimal precio = 100.00m;
            // Act
            await ServicioCategoriaRepository.AgregarPrecioServicioCategoriaAsync(servicioId, categoriaId, precio);
            // Assert
            var resultado = await ServicioCategoriaRepository.ObtenerPrecioServicioCategoriaEspecificoAsync(servicioId, categoriaId);
            Assert.NotNull(resultado);
            Assert.Equal(precio, resultado.Precio);
        }
        [Fact]
        public async Task ActualizarPrecioServicioCategoriaAsync_DeberiaActualizarCorrectamente()
        {
            // Arrange
            int servicioId = 1;
            int categoriaId = 2;
            decimal nuevoPrecio = 150.00m;
            // Act
            await ServicioCategoriaRepository.ActualizarPrecioServicioCategoriaAsync(servicioId, categoriaId, nuevoPrecio);
            // Assert
            var resultado = await ServicioCategoriaRepository.ObtenerPrecioServicioCategoriaEspecificoAsync(servicioId, categoriaId);
            Assert.NotNull(resultado);
            Assert.Equal(nuevoPrecio, resultado.Precio);
        }
        [Fact]
        public async Task EliminarPrecioServicioCategoriaAsync_DeberiaEliminarCorrectamente()
        {
            // Arrange
            int servicioId = 1;
            int categoriaId = 2;
            // Act
            await ServicioCategoriaRepository.EliminarPrecioServicioCategoriaAsync(servicioId, categoriaId);
            // Assert
            var resultado = await ServicioCategoriaRepository.ObtenerPrecioServicioCategoriaEspecificoAsync(servicioId, categoriaId);
            Assert.Null(resultado);
        }
        [Fact]
        public async Task ObtenerPreciosPorCategoriaAsync_DeberiaRetornarPreciosCorrectos()
        {
            // Arrange
            int categoriaId = 2;
            // Act
            var resultados = await ServicioCategoriaRepository.ObtenerPreciosPorCategoriaAsync(categoriaId);
            // Assert
            Assert.NotEmpty(resultados);
            foreach (var resultado in resultados)
            {
                Assert.Equal(categoriaId, resultado.IdCategoriaHabitacion);
            }
        }
        [Fact]
        public async Task ObtenerPreciosPorServicioAsync_DeberiaRetornarPreciosCorrectos()
        {
            // Arrange
            int servicioId = 1;
            // Act
            var resultados = await ServicioCategoriaRepository.ObtenerPreciosPorServicioAsync(servicioId);
            // Assert
            Assert.NotEmpty(resultados);
            foreach (var resultado in resultados)
            {
                Assert.Equal(servicioId, resultado.IdServicio);
            }
        }
    }
}

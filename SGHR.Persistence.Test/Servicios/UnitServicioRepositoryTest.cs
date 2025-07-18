using SGHR.Persistence.Test.TestBase;
using ServicioE = SGHR.Domain.Entities.Servicios.Servicios;
using SGHR.Persistence.Repositories.Servicios;

namespace SGHR.Persistence.Test.Servicios
{
    public class UnitServicioRepositoryTest : UnitRepositoryTestBase
    {
        [Fact]
        public async Task AgregarServicio_DeberiaGuardarCorrectamente()
        {
            // Arrange
            var nuevoServicio = new ServicioE("Servicio de prueba", "Descripción del servicio");

            // Act
            await ServicioRepository.AgregarServicioAsync(nuevoServicio);
            await Context.SaveChangesAsync();
            
            // Assert
            var servicioGuardado = await Context.Servicios.FindAsync(nuevoServicio.Id);
            Assert.NotNull(servicioGuardado);
            Assert.Equal(nuevoServicio.Nombre, servicioGuardado.Nombre);
            Assert.Equal(nuevoServicio.Descripcion, servicioGuardado.Descripcion);
        }
        [Fact]
        public async Task ActualizarServicio_DeberiaActualizarCorrectamente()
        {
            // Arrange
            var servicioExistente = new ServicioE("Servicio Original", "Descripción original");
            Context.Servicios.Add(servicioExistente);
            await Context.SaveChangesAsync();
            servicioExistente.Actualizar("Servicio Actualizado", "Descripción actualizada");
            // Act
            await ServicioRepository.ActualizarServicioAsync(servicioExistente);
            await Context.SaveChangesAsync();
            // Assert
            var servicioActualizado = await Context.Servicios.FindAsync(servicioExistente.Id);
            Assert.NotNull(servicioActualizado);
            Assert.Equal("Servicio Actualizado", servicioActualizado.Nombre);
            Assert.Equal("Descripción actualizada", servicioActualizado.Descripcion);
        }
        [Fact]
        public async Task EliminarServicio_DeberiaEliminarCorrectamente()
        {
            // Arrange
            var servicioExistente = new ServicioE("Servicio a eliminar", "Descripción del servicio a eliminar");
            Context.Servicios.Add(servicioExistente);
            await Context.SaveChangesAsync();
            
            // Act
            await ServicioRepository.EliminarServicioAsync(servicioExistente.Id);
            await Context.SaveChangesAsync();
            
            // Assert
            var servicioEliminado = await Context.Servicios.FindAsync(servicioExistente.Id);
            Assert.Null(servicioEliminado);
        }
        [Fact]
        public async Task ObtenerPorIdAsync_DeberiaRetornarServicioExistente()
        {
            // Arrange
            var servicioExistente = new ServicioE("Servicio de prueba", "Descripción del servicio de prueba");
            Context.Servicios.Add(servicioExistente);
            await Context.SaveChangesAsync();
            
            // Act
            var servicioObtenido = await ServicioRepository.ObtenerPorIdAsync(servicioExistente.Id);
            
            // Assert
            Assert.NotNull(servicioObtenido);
            Assert.Equal(servicioExistente.Id, servicioObtenido.Id);
            Assert.Equal(servicioExistente.Nombre, servicioObtenido.Nombre);
        }
        [Fact]
        public async Task ObtenerServiciosActivosAsync_DeberiaRetornarSoloServiciosActivos()
        {
            // Arrange
            var servicioActivo = new ServicioE("Servicio Activo", "Descripción del servicio activo");
            servicioActivo.Activar();
            Context.Servicios.Add(servicioActivo);
            
            var servicioInactivo = new ServicioE("Servicio Inactivo", "Descripción del servicio inactivo");
            servicioInactivo.Desactivar();
            Context.Servicios.Add(servicioInactivo);
            
            await Context.SaveChangesAsync();
            
            // Act
            var serviciosActivos = await ServicioRepository.ObtenerServiciosActivosAsync();
            
            // Assert
            Assert.Single(serviciosActivos);
            Assert.Contains(servicioActivo, serviciosActivos);
            Assert.DoesNotContain(servicioInactivo, serviciosActivos);
        }
        [Fact]
        public async Task ObtenerTodosLosServiciosAsync_DeberiaRetornarTodosLosServicios()
        {
            // Arrange
            var servicio1 = new ServicioE("Servicio 1", "Descripción del servicio 1");
            var servicio2 = new ServicioE("Servicio 2", "Descripción del servicio 2");
            Context.Servicios.Add(servicio1);
            Context.Servicios.Add(servicio2);
            await Context.SaveChangesAsync();
            
            // Act
            var todosLosServicios = await ServicioRepository.ObtenerTodosLosServiciosAsync();
            
            // Assert
            Assert.Equal(2, todosLosServicios.Count);
            Assert.Contains(servicio1, todosLosServicios);
            Assert.Contains(servicio2, todosLosServicios);
        }

    }
}

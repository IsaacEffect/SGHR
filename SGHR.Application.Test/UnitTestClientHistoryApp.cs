using Microsoft.Extensions.Logging;
using SGHR.Application.Services;
using SGHR.Application.Test.Utils;
using SGHR.Domain.Entities.Historial;

namespace SGHR.Application.Test
{
    public class UnitTestClientHistoryApp
    {
        private static HistorialReserva CrearReserva(int id, int clienteId, string estado, string tipoHabitacion)
        {
            return new HistorialReserva
            {
                Id = id,
                ClienteId = clienteId,
                Estado = estado,
                TipoHabitacion = tipoHabitacion,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(2),
                Tarifa = 200,
                ServiciosAdicionales = "WiFi"
            };
        }

        private HistorialReservaService CrearServicio(out FakeHistorialReservaRepository repo)
        {
            repo = new FakeHistorialReservaRepository();
            var logger = LoggerFactory.Create(builder => { }).CreateLogger<HistorialReservaService>();
            return new HistorialReservaService(repo, logger);
        }

        [Fact]
        public async Task ObtenerHistorialAsync_DeberiaRetornarResultados()
        {
            var servicio = CrearServicio(out var repo);
            repo.AgregarReserva(CrearReserva(1, 10, "Confirmada", "Suite"));
            repo.AgregarReserva(CrearReserva(2, 10, "Cancelada", "Doble"));

            var resultado = await servicio.ObtenerHistorialAsync(10);

            Assert.True(resultado.Success);
            Assert.Equal(2, resultado.Data.Count());
        }

        [Fact]
        public async Task ObtenerHistorialAsync_DeberiaRetornarVacioSiNoHayDatos()
        {
            var servicio = CrearServicio(out var _);
            var resultado = await servicio.ObtenerHistorialAsync(999);

            Assert.True(resultado.Success);
            Assert.Empty(resultado.Data);
        }

        [Fact]
        public async Task ObtenerDetalleAsync_DeberiaRetornarReservaCorrecta()
        {
            var servicio = CrearServicio(out var repo);
            repo.AgregarReserva(CrearReserva(1, 10, "Confirmada", "Suite"));

            var resultado = await servicio.ObtenerDetalleAsync(1, 10);

            Assert.True(resultado.Success);
            Assert.Equal("Suite", resultado.Data.TipoHabitacion);
        }

        [Fact]
        public async Task ObtenerDetalleAsync_DeberiaRetornarErrorSiNoExiste()
        {
            var servicio = CrearServicio(out var _);
            var resultado = await servicio.ObtenerDetalleAsync(99, 999);

            Assert.False(resultado.Success);
            Assert.Equal("Error al obtener detalle de la reserva.", resultado.Message);
        }

        [Fact]
        public async Task ObtenerHistorialAsync_DeberiaFiltrarPorEstado()
        {
            var servicio = CrearServicio(out var repo);
            repo.AgregarReserva(CrearReserva(1, 10, "Confirmada", "Suite"));
            repo.AgregarReserva(CrearReserva(2, 10, "Cancelada", "Doble"));

            var resultado = await servicio.ObtenerHistorialAsync(10, null, null, "Confirmada");

            Assert.True(resultado.Success);
            Assert.Single(resultado.Data);
            Assert.Equal("Confirmada", resultado.Data.First().Estado);
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Persistence.Repositories;
using SGHR.Persistence.Test.Utils;

namespace SGHR.Persistence.Test
{
    public class UnitTestClientHistoryPersist
    {
        private HistorialReservaRepository CrearRepositorio()
        {
            var config = new ConfigurationBuilder().Build();
            var logger = LoggerFactory.Create(builder => { }).CreateLogger<HistorialReservaRepository>();
            var helper = new FakeSqlHelper();
            return new HistorialReservaRepository(config, logger, helper);
        }

        [Fact]
        public async Task GetHistorialByClienteAsync_DeberiaRetornarReservas()
        {
            var repo = CrearRepositorio();
            var resultado = await repo.GetHistorialByClienteAsync(10);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
        }

        [Fact]
        public async Task GetDetalleReservaAsync_DeberiaRetornarReservaCorrecta()
        {
            var repo = CrearRepositorio();
            var resultado = await repo.GetDetalleReservaAsync(1, 10);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("Suite", resultado.TipoHabitacion);
        }

        [Fact]
        public async Task GetDetalleReservaAsync_DeberiaRetornarNullSiNoHayCoincidencia()
        {
            var repo = CrearRepositorio();
            var resultado = await repo.GetDetalleReservaAsync(99, 999);

            Assert.Null(resultado);
        }
    }
}

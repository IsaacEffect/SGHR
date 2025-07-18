using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.Reservas;
using SGHR.Persistence.Repositories.Servicios;
namespace SGHR.Persistence.Test.TestBase
{
    public class UnitRepositoryTestBase : IDisposable
    {
        protected readonly SGHRDbContext Context;
        protected readonly ReservaRepository ReservaRepository;
        protected readonly ServicioRepository ServicioRepository;
        protected readonly ServicioCategoriaRepository ServicioCategoriaRepository;

        protected UnitRepositoryTestBase()
        {
            Context = SGHRDbContextFactoryTest.CreateInMemoryDbContext();
            ReservaRepository = new ReservaRepository(Context, new FakeSqlConnectionFactory());
            ServicioRepository = new ServicioRepository(Context);
            ServicioCategoriaRepository = new ServicioCategoriaRepository(new FakeSqlConnectionFactory());
            SeedData();
        }

        protected virtual void SeedData() { }
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}

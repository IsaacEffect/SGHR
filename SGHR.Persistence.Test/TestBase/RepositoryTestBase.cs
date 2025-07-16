using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.Reservas;
using SGHR.Persistence.Repositories.Servicios;
namespace SGHR.Persistence.Test.TestBase
{
    public class RepositoryTestBase : IDisposable
    {
        protected readonly SGHRDbContext Context;
        protected readonly ReservaRepository ReservaRepository;
        protected readonly ServicioRepository ServicioRepository;
        
        protected RepositoryTestBase()
        {
            Context = SGHRDbContextFactory.CreateInMemoryDbContext();
            ReservaRepository = new ReservaRepository(Context, new FakeSqlConnectionFactory());
            ServicioRepository = new ServicioRepository(Context, new FakeSqlConnectionFactory());

            SeedData();
        }

        protected virtual void SeedData() { }
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}

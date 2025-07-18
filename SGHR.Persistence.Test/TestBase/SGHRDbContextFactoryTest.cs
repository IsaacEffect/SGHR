using Microsoft.EntityFrameworkCore;
using SGHR.Persistence.Context;
namespace SGHR.Persistence.Test.TestBase
{
    public class SGHRDbContextFactoryTest
    {
        public static SGHRDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<SGHRDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new SGHRDbContext(options);
        }
    }
}

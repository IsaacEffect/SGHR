
using Microsoft.EntityFrameworkCore;
using SGHR.Persistence.Domain;

namespace SGHR.Persistence.Context
{
    public class SGHRContext : DbContext
    {
        public SGHRContext(DbContextOptions<SGHRContext> options)
            : base(options) { }

        public DbSet<Piso> Pisos { get; set; }
    }
}

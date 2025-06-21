using Microsoft.EntityFrameworkCore;
using SGHR.Domain.Entities.Clientes;
using SGHR.Persistence.Configurations;

namespace SGHR.Persistence.Context
{
    public class HotelReservaDBContext : DbContext
    {
        public HotelReservaDBContext(DbContextOptions<HotelReservaDBContext> options) 
            : base(options)
        {

        }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        }
    }
}

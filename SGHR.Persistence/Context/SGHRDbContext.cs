using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGHR.Domain.Entities.Reservas;
using SGHR.Domain.Entities.Servicios;
using SGHR.Domain.Entities.Habitaciones;
using SGHR.Domain.Entities.Users;
namespace SGHR.Persistence.Context
{
    public class SGHRDbContext : DbContext
    {
        
        public SGHRDbContext(DbContextOptions<SGHRDbContext> options) : base(options) { }

        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Servicios> Servicios { get; set; }
        public DbSet<CategoriaHabitacion> CategoriaHabitaciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<ServicioCategoria> ServicioCategorias { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

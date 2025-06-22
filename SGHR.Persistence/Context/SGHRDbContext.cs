using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGHR.Domain.Entities.Reservas;
using SGHR.Domain.Entities.Servicios;

namespace SGHR.Persistence.Context
{
    public class SGHRDbContext: DbContext
    {
        public SGHRDbContext(DbContextOptions<SGHRDbContext> options) : base(options)
        {
        }

        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Servicio> Servicios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

        }
    }
}

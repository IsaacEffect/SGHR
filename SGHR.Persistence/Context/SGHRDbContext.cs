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
        public DbSet<CategoriaHabitacion> CategoriaHabitacion { get; set; }
        public DbSet<ServicioCategoria> ServicioCategorias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   

            modelBuilder.Entity<Cliente>(entity =>  
            {
                entity.ToTable("Clientes");
                entity.Property(u => u.Id).HasColumnName("IdCliente").ValueGeneratedOnAdd();
                entity.Property(u => u.Nombre).HasColumnName("Nombre");
                entity.Property(u => u.Apellido).HasColumnName("Apellido").HasMaxLength(100);
                entity.Property(u => u.HashedPassword).HasColumnName("ContrasenaHashed");
                entity.Property(u => u.Email).HasColumnName("Email");
                entity.Property(u => u.Rol).HasColumnName("Rol").HasConversion<string>();
                entity.Property(u => u.FechaCreacion).HasColumnName("FechaRegistro");
                entity.Property(u => u.Activo).HasColumnName("Estado");
                entity.Property(u => u.FechaModificacion).HasColumnName("FechaUltimaModificacion");
                entity.Property(u => u.Telefono).HasColumnName("Telefono").HasMaxLength(50);
                entity.Property(u => u.Direccion).HasColumnName("Direccion").HasMaxLength(255);
            });
            modelBuilder.Entity<Servicios>(entity =>
            {
                entity.ToTable("Servicios");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).HasColumnName("IdServicio");
                entity.Property(s => s.Nombre).HasColumnName("NombreServicio").IsRequired().HasMaxLength(100);
                entity.Property(s => s.Descripcion).HasColumnName("Descripcion").IsRequired().HasMaxLength(500);
                entity.Property(s => s.Activo).HasColumnName("Estado");
                entity.Property(s => s.FechaCreacion).HasColumnName("FechaCreacion");
                entity.Property(s => s.FechaModificacion).HasColumnName("FechaUltimaModificacion");
            });
            modelBuilder.Entity<CategoriaHabitacion>(entity =>
            {
                entity.ToTable("CategoriasHabitacion");
                entity.HasKey(ch => ch.Id);
                entity.Property(ch => ch.Id).HasColumnName("IdCategoriaHabitacion").ValueGeneratedOnAdd();
                entity.Property(ch => ch.Nombre).HasColumnName("Nombre");
                entity.Property(ch => ch.Descripcion).HasColumnName("Descripcion").HasMaxLength(500);
                entity.Property(ch => ch.TarifaBase).HasColumnName("TarifaBase").HasColumnType("decimal(10,2)");
                entity.Property(ch => ch.Caracteristicas).HasColumnName("Caracteristicas").HasMaxLength(1000);
                entity.Property(ch => ch.Estado).HasColumnName("Estado");

            });
            modelBuilder.Entity<ServicioCategoria>(entity =>
            {
                entity.ToTable("ServicioCategoria");
                entity.HasKey(sc => new { sc.IdServicio, sc.IdCategoriaHabitacion });
                entity.Property(sc => sc.PrecioServicio).HasColumnName("PrecioServicio").HasColumnType("decimal(10,2)");
                
                entity.Property(sc => sc.FechaCreacion).HasColumnName("FechaCreacion");
                entity.Property(sc => sc.FechaModificacion).HasColumnName("FechaActualizacion");
            });
            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.ToTable("Reservas");

                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).HasColumnName("IdReserva").IsRequired().ValueGeneratedOnAdd(); 

                entity.Property(r => r.ClienteId).HasColumnName("IdCliente").IsRequired().ValueGeneratedOnAdd();
                entity.Property(r => r.IdCategoriaHabitacion).HasColumnName("IdCategoriaHabitacion").IsRequired();
                entity.Property(r => r.FechaEntrada).HasColumnName("FechaEntrada").IsRequired();
                entity.Property(r => r.FechaSalida).HasColumnName("FechaSalida").IsRequired();

                entity.Property(r => r.Estado).HasColumnName("EstadoReserva").IsRequired().HasConversion<int>();

                entity.Property(r => r.FechaCreacion).HasColumnName("FechaReserva").IsRequired();
                entity.Property(r => r.FechaModificacion).HasColumnName("FechaModificacion");
                entity.Property(r => r.Activo).HasColumnName("Activo").IsRequired();

                 entity.Property(r => r.NumeroHuespedes).HasColumnName("NumeroHuespedes").IsRequired();
                 entity.Property(r => r.NumeroReservaUnico).HasColumnName("NumeroReservaUnico").IsRequired().HasMaxLength(50);
                 entity.Property(r => r.FechaCancelacion).HasColumnName("FechaCancelacion").IsRequired(false);
                 entity.Property(r => r.MotivoCancelacion).HasColumnName("MotivoCancelacion").HasMaxLength(500);
                
                entity.HasOne(r => r.Cliente)
                      .WithMany()
                      .HasForeignKey(r => r.ClienteId)
                      .IsRequired();

                entity.HasOne(r => r.CategoriaHabitacion)
                      .WithMany()
                      .HasForeignKey(r => r.IdCategoriaHabitacion)
                      .IsRequired();
            });

                modelBuilder.Entity<ServicioCategoria>()
                     .HasOne(sc => sc.Servicios)
                     .WithMany(s => s.ServicioCategorias)
                     .HasForeignKey(sc => sc.IdServicio);

                modelBuilder.Entity<ServicioCategoria>()
                       .HasOne(sc => sc.CategoriaHabitacion)
                       .WithMany(ch => ch.ServicioCategorias)
                       .HasForeignKey(sc => sc.IdCategoriaHabitacion);

                base.OnModelCreating(modelBuilder);
            
        }
    }
}

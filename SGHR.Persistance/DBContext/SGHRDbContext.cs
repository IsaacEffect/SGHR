using Microsoft.EntityFrameworkCore;
using SGHR.Persistance.Models;
using SGHR.WebApp.Api;


namespace SGHR.Persistance.DBContext;

public partial class SGHRDbContext : DbContext
{
    public SGHRDbContext()
    {
    }

    public SGHRDbContext(DbContextOptions<SGHRDbContext> options): base(options)
    {
    }

    public virtual DbSet<CategoriasHabitacion> CategoriasHabitaciones { get; set; }

    public virtual DbSet<Tarifa> Tarifas { get; set; }

    public virtual DbSet<Habitaciones> Habitaciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriasHabitacion>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaHabitacion).HasName("PK__Categori__74FB8F9AC497D9AE");

            entity.ToTable("CategoriasHabitacion");

            entity.HasIndex(e => e.Nombre, "UQ__Categori__75E3EFCFC61C39AD").IsUnique();

            entity.Property(e => e.Caracteristicas).HasMaxLength(1000);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.TarifaBase).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Tarifa>(entity =>
        {
            entity.HasKey(e => e.IdTarifa).HasName("PK__Tarifas__78F1A91DBA52A1F2");

            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TipoTemporada).HasMaxLength(50);

            entity.HasOne(d => d.IdCategoriaHabitacionNavigation).WithMany(p => p.Tarifas)
                .HasForeignKey(d => d.IdCategoriaHabitacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tarifa_Categoria");
        });

        modelBuilder.Entity<Habitaciones>()
    .HasOne(h => h.CategoriaHabitacion)
    .WithMany(c => c.Habitaciones)
    .HasForeignKey(h => h.IdCategoriaHabitacion);


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

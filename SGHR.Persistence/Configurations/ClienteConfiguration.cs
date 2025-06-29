using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGHR.Domain.Entities.Clientes;

namespace SGHR.Persistence.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("IdCliente");
            builder.Property(c => c.Nombre).HasColumnName("Nombre");
            builder.Property(c => c.Apellido).HasColumnName("Apellido");
            builder.Property(c => c.Correo).HasColumnName("Email");
            builder.Property(c => c.Contrasena).HasColumnName("ContrasenaHashed");
            builder.Property(c => c.Direccion).HasColumnName("Direccion");
            builder.Property(c => c.Telefono).HasColumnName("Telefono");
            builder.Property(c => c.FechaRegistro).HasColumnName("FechaRegistro");
            builder.Property(c => c.Rol).HasColumnName("Rol");

            builder.HasMany(c => c.Historial)
                   .WithOne(h => h.Cliente)
                   .HasForeignKey(h => h.ClienteId);
        }
    }
}

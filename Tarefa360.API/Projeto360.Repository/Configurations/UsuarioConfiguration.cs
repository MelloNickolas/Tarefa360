using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(u => u.ID);

        builder.Property(u => u.Nome).IsRequired(true).HasMaxLength(150);
        builder.Property(u => u.Email).IsRequired(true).HasMaxLength(254);
        builder.Property(u => u.Senha).IsRequired(true).HasMaxLength(255);
        builder.Property(u => u.Ativo).IsRequired(true);
        builder.Property(u => u.TipoUsuario).IsRequired(true);

        builder.HasIndex(u => u.Email).IsUnique();
    }
}
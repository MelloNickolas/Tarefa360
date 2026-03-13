using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto360.Dominio.Entidades;


namespace Projeto360.Repositories.Configs
{
  public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
  {
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
      builder.ToTable("Usuarios").HasKey(u => u.IDUsuario);

      builder.Property(nameof(Usuario.IDUsuario)).HasColumnName("IDUsuario").ValueGeneratedOnAdd();;
      builder.Property(nameof(Usuario.Nome)).HasColumnName("Nome").IsRequired();
      builder.Property(nameof(Usuario.Email)).HasColumnName("Email").IsRequired();
      builder.Property(nameof(Usuario.Senha)).HasColumnName("Senha").IsRequired();
      builder.Property(nameof(Usuario.Ativo)).HasColumnName("Ativo").HasDefaultValue(true).IsRequired();
      builder.Property(nameof(Usuario.TipoUsuario))
       .HasColumnName("TipoUsuario")
       .IsRequired();
    }
  }
}
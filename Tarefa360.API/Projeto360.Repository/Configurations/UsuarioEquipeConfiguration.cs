using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Configurations;

public class UsuarioEquipeConfiguration : IEntityTypeConfiguration<UsuarioEquipe>
{
  public void Configure(EntityTypeBuilder<UsuarioEquipe> builder)
  {
    builder.ToTable("UsuariosEquipes");
    builder.HasKey(usuarioEquipe => usuarioEquipe.ID);

    builder
    .HasOne(UsuarioEquipe => UsuarioEquipe.Usuario)
    .WithMany(usuario => usuario.UsuarioEquipes)
    .HasForeignKey(usuarioTarefa => usuarioTarefa.UsuarioId);


    builder
    .HasOne(UsuarioEquipe => UsuarioEquipe.Equipe)
    .WithMany(equipe => equipe.UsuariosEquipe)
    .HasForeignKey(usuarioTarefa => usuarioTarefa.EquipeId);
  }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Configurations;

public class EquipeConfiguration : IEntityTypeConfiguration<Equipe>
{
  public void Configure(EntityTypeBuilder<Equipe> builder)
  {
    builder.ToTable("Equipes");
    builder.HasKey(equipe => equipe.ID);

    builder.Property(equipe => equipe.Nome).IsRequired(true).HasMaxLength(100);
  }
}
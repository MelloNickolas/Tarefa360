using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Configurations;

public class HistoriaConfiguration : IEntityTypeConfiguration<Historia>
{
    public void Configure(EntityTypeBuilder<Historia> builder)
    {
        builder.ToTable("Historias");
        builder.HasKey(historia => historia.ID);

        builder.Property(historia => historia.Nome).IsRequired(true).HasMaxLength(100);
        builder.Property(historia => historia.Descricao).IsRequired(false).HasMaxLength(500);

        builder
          .HasOne(Historia => Historia.Projeto)
          .WithMany(Projeto => Projeto.Historias)
          .HasForeignKey(historia => historia.ProjetoID);
    }
}
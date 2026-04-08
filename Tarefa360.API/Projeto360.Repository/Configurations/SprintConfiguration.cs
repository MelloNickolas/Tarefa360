using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Configurations;

public class SprintConfiguration : IEntityTypeConfiguration<Sprint>
{
    public void Configure(EntityTypeBuilder<Sprint> builder)
    {
        builder.ToTable("Sprints");

        builder.HasKey(s => s.ID);

        builder.Property(s => s.Titulo)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.Descricao)
               .HasMaxLength(500);

        builder.Property(s => s.DataInicio)
               .IsRequired();

        builder.Property(s => s.DataFim)
               .IsRequired();

        builder
            .HasOne(s => s.Projeto)
            .WithMany(p => p.Sprints)
            .HasForeignKey(s => s.ProjetoID);
    }
}
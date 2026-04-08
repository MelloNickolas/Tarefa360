using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Configurations;

public class ProjetoConfiguration : IEntityTypeConfiguration<Projeto>
{
    public void Configure(EntityTypeBuilder<Projeto> builder)
    {
        builder.ToTable("Projetos");
        builder.HasKey(u => u.ID);

        builder.Property(u => u.Nome).IsRequired(true).HasMaxLength(150);
        builder.Property(u => u.Descricao).IsRequired(true).HasMaxLength(500);
        builder.Property(u => u.Ativo).IsRequired(true);
    }
}
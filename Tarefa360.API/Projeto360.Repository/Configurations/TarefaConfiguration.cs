using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Configurations;

public class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.ToTable("Tarefas");
        builder.HasKey(tarefa => tarefa.ID);

        builder.Property(tarefa => tarefa.Titulo).IsRequired(true).HasMaxLength(100);
        builder.Property(tarefa => tarefa.Descricao).IsRequired(false).HasMaxLength(500);
        builder.Property(tarefa => tarefa.Estimativa);
        builder.Property(tarefa => tarefa.Concluido).IsRequired(true);
        builder.Property(tarefa => tarefa.TipoTarefa).IsRequired(true);
        builder.Property(tarefa => tarefa.DataCriacao)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");
        builder.Property(tarefa => tarefa.DataConclusao).IsRequired(false);

        builder
          .HasOne(tarefa => tarefa.Projeto)
          .WithMany(Projeto => Projeto.Tarefas)
          .HasForeignKey(tarefa => tarefa.ProjetoID);
        
        builder
          .HasOne(tarefa => tarefa.Historia)
          .WithMany(Historia => Historia.Tarefas)
          .HasForeignKey(tarefa => tarefa.HistoriaID);

        builder
          .HasOne(tarefa => tarefa.Sprint)
          .WithMany(Sprint => Sprint.Tarefas)
          .HasForeignKey(tarefa => tarefa.SprintID);
    }
}
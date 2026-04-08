using Microsoft.EntityFrameworkCore;
using Projeto360.Domain.Entities;
using Projeto360.Domain.Enums;
using Projeto360.Repository.Configurations;

public class Projeto360Context : DbContext
{
    private readonly DbContextOptions _options;

    public Projeto360Context(DbContextOptions<Projeto360Context> options)
    : base(options)
    {
        _options = options;
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Projeto> Projetos { get; set; }
    public DbSet<Historia> Historias { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }
    public DbSet<Sprint> Sprints { get; set; }
    public DbSet<Equipe> Equipes { get; set; }
    public DbSet<UsuarioEquipe> UsuariosEquipes { get; set; }
    public DbSet<TwoFactorToken> TwoFactorTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new ProjetoConfiguration());
        modelBuilder.ApplyConfiguration(new HistoriaConfiguration());

        modelBuilder.Entity<TwoFactorToken>(entity =>
        {
            entity.HasKey(t => t.ID);

            // Código sempre com 6 caracteres
            entity.Property(t => t.Codigo).IsRequired().HasMaxLength(6);
            entity.Property(t => t.Expiracao).IsRequired();

            // Quando o usuário for deletado, apaga os tokens dele também
            entity.HasOne(t => t.Usuario)
            .WithMany(u => u.TwoFactorTokens)
            .HasForeignKey(t => t.UsuarioID)
            .OnDelete(DeleteBehavior.Cascade);
        });

        // Campos opcionais de RefreshToken na tabela Usuarios
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(u => u.RefreshToken).IsRequired(false);
            entity.Property(u => u.RefreshTokenExpiracao).IsRequired(false);
        });

        modelBuilder.ApplyConfiguration(new TarefaConfiguration());
        modelBuilder.ApplyConfiguration(new SprintConfiguration());
        modelBuilder.Entity<Usuario>().HasData(

        //Cria um usuário padrão para fazer alterações e visualizar o código em criação
        new Usuario
        {
            ID = 1,
            Nome = "Administrador",
            Email = "admin.tarefa360@gmail.com",
            Senha = BCrypt.Net.BCrypt.HashPassword("Admin.123"),
            Ativo = true,
            TipoUsuario = TiposUsuario.Administrador
        });

        modelBuilder.ApplyConfiguration(new EquipeConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioEquipeConfiguration());
    }
}
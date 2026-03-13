using Projeto360.Dominio.Entidades;
using Projeto360.Repositories.Configs;
using Microsoft.EntityFrameworkCore;


public class Projeto360Context : DbContext
{

  public DbSet<Usuario> Usuarios { get; set; }


  // Construtor para a string de conexao
  public readonly DbContextOptions _options;
  public Projeto360Context()
  {}
  public Projeto360Context(DbContextOptions options) : base(options)
  {
    _options = options;
  }


  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (_options == null)
      optionsBuilder.UseSqlite(@"Filename=./projetos360.sqlite;");
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfiguration(new UsuarioConfig());
  }
}
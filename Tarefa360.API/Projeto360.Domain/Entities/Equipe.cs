namespace Projeto360.Domain.Entities;

public class Equipe
{
  public int ID { get; set; }
  public string Nome { get; set; }


  // campo para relacionamento com UsuarioEquipe
  public ICollection<UsuarioEquipe> UsuariosEquipe;
  
}
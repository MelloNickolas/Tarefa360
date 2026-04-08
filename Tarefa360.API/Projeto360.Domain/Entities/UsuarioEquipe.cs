using Projeto360.Domain.Enums;

namespace Projeto360.Domain.Entities;

public class UsuarioEquipe
{
  public int ID { get; set; }
  public PapeisEquipe PapeisEquipe { get; set; }


  // Puxando o relacionamento com o usuário
  public int UsuarioId { get; set; }
  public Usuario Usuario { get; set; }

  // Puxando o relacionamento com a equipe
  public int EquipeId { get; set; }
  public Equipe Equipe { get; set; }
}
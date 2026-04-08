using Projeto360.Domain.Enums;

namespace Projeto360.Api.Models.Request;

public class UsuarioEquipeCriar
{
  public PapeisEquipe PapeisEquipe { get; set; }
  public int UsuarioID { get; set; }
  public int EquipeID { get; set; }
}
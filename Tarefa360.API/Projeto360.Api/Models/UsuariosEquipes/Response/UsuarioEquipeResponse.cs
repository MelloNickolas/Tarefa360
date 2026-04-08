using Projeto360.Domain.Enums;

namespace Projeto360.Api.Models.Response;

public class UsuarioEquipeResponse
{
  public int ID { get; set; }
  public PapeisEquipe PapeisEquipe { get; set; }
  public int UsuarioID { get; set; }
  public int EquipeID { get; set; }
  public string NomeUsuario { get; set; }
  public string NomeEquipe { get; set; }
}
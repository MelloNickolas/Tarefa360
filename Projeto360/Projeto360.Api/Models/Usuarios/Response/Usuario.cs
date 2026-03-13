using Projeto360.Dominio.Enumeradores;

namespace Projeto360.Api.Models.Response;

public class UsuarioResponse
{
  public int IDUsuario {get; set;}
  public string Nome { get; set; }
  public string Email { get; set; }
  public TipoUsuario TipoUsuario { get; set; }
}
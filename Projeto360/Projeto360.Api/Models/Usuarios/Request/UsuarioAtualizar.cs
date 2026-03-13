using Projeto360.Dominio.Enumeradores;

namespace Projeto360.Api.Models.Request;

public class UsuarioAtualizar
{
  public int IDUsuario { get; set; }
  public string Nome { get; set; }
  public string Email { get; set; }
  public TipoUsuario TipoUsuario { get; set; }
}
using Projeto360.Dominio.Enumeradores;

namespace Projeto360.Api.Models.Request;

public class UsuarioCriar
{
  public string Nome { get; set; }
  public string Email { get; set; }
  public string Senha { get; set; }
  public TipoUsuario TipoUsuario { get; set; }
}
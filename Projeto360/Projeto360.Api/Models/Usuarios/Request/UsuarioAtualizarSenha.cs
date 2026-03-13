namespace Projeto360.Api.Models.Request;

public class UsuarioAtualizarSenha
{
  public int IDUsuario { get; set; }
  public string Senha { get; set; }
  public string SenhaAntiga { get; set; }
}
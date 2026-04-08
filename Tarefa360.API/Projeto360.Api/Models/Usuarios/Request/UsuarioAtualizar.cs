namespace Projeto360.Api.Models.Request;

public class UsuarioAtualizar
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public int TipoUsuarioId { get; set; }
}
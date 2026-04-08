namespace Projeto360.Api.Models.Response;

public class UsuarioResponse
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public int TipoUsuarioId { get; set; }
}
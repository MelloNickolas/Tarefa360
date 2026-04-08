namespace Projeto360.Api.Models.Request;

public class UsuarioAlterarSenha
{
    public int ID { get; set; }
    public string novaSenha { get; set; }
    public string senhaAtual { get; set; }
}
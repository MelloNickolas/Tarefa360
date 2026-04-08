namespace Projeto360.Api.Models.Request;

public class UsuarioConfirmarCodigoRequest
{
    // E-mail identifica o usuário sem precisar de sessão nem cookie
    public string Email { get; set; }
    // Código de 6 dígitos que o usuário recebeu por e-mail
    public string Codigo { get; set; }
}
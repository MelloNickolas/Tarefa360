namespace Projeto360.Api.Models.Request;

public class UsuarioIniciarLoginRequest
{
    public string Email { get; set; }
    public string Senha { get; set; }
    // Token gerado pelo widget reCAPTCHA no frontend e enviado para validação
    public string CaptchaToken { get; set; }
}
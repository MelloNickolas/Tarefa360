namespace Projeto360.Api.Models.Request;

public class UsuarioRenovarTokenRequest
{
    // O RefreshToken que o frontend salvou no localStorage após o login
    public string RefreshToken { get; set; }
}
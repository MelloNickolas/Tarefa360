namespace Projeto360.Application.Interfaces;

public interface IAutenticacaoApplication
{
    // PASSO 1 DO LOGIN: valida captcha + credenciais e envia código por e-mail
    Task IniciarLoginAsync(string email, string senha, string captchaToken);

    // PASSO 2 DO LOGIN: valida o código de 6 dígitos e retorna os dois tokens
    Task<(string AccessToken, string RefreshToken)> ConfirmarCodigoDoisFatoresAsync(
        string email, string codigo);

    // RENOVAÇÃO: troca um RefreshToken ainda válido por um par novo de tokens
    Task<(string AccessToken, string RefreshToken)> RenovarTokenAsync(string refreshToken);

    // LOGOUT: apaga o RefreshToken do banco para invalidar a sessão
    Task RevogarTokenAsync(string refreshToken);
}
using Microsoft.AspNetCore.Mvc;
using Projeto360.Api.Models.Request;
using Projeto360.Application.Interfaces;

namespace Projeto360.Api.Controllers;

[ApiController]
[Route("[controller]")]  // Rota base: /auth
public class AuthController : ControllerBase
{
    private readonly IAutenticacaoApplication _autenticacaoApplication;

    public AuthController(IAutenticacaoApplication autenticacaoApplication)
    {
        _autenticacaoApplication = autenticacaoApplication;
    }

    // POST /auth/iniciar-login
    // Recebe e-mail + senha + captcha → valida tudo e envia o código por e-mail
    [HttpPost("iniciar-login")]
    public async Task<IActionResult> IniciarLogin([FromBody] UsuarioIniciarLoginRequest request)
    {
        try
        {
            await _autenticacaoApplication.IniciarLoginAsync(
                request.Email,
                request.Senha,
                request.CaptchaToken
            );

            // Resposta genérica — não confirma nem nega se o e-mail existe
            // (evita que atacantes descubram quais e-mails estão cadastrados)
            return Ok(new { mensagem = "Código de verificação enviado para seu e-mail." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    // POST /auth/confirmar-codigo
    // Recebe e-mail + código de 6 dígitos → valida e devolve AccessToken + RefreshToken
    [HttpPost("confirmar-codigo")]
    public async Task<IActionResult> ConfirmarCodigo([FromBody] UsuarioConfirmarCodigoRequest request)
    {
        try
        {
            var (accessToken, refreshToken) =
                await _autenticacaoApplication.ConfirmarCodigoDoisFatoresAsync(
                    request.Email,
                    request.Codigo
                );

            // Devolve os dois tokens — o frontend salva ambos no localStorage
            return Ok(new { accessToken, refreshToken });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { mensagem = ex.Message });
        }
    }

    // POST /auth/renovar-token
    // Recebe o RefreshToken → valida e devolve um novo par de tokens
    // Chamado automaticamente pelo interceptor do Axios quando AccessToken expira
    [HttpPost("renovar-token")]
    public async Task<IActionResult> RenovarToken([FromBody] UsuarioRenovarTokenRequest request)
    {
        try
        {
            var (accessToken, refreshToken) =
                await _autenticacaoApplication.RenovarTokenAsync(request.RefreshToken);

            return Ok(new { accessToken, refreshToken });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { mensagem = ex.Message });
        }
    }

    // POST /auth/logout
    // Reutiliza RenovarTokenRequest pois precisa apenas do RefreshToken
    // Invalida o token no banco — qualquer tentativa de renovação futura será rejeitada
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] UsuarioRenovarTokenRequest request)
    {
        await _autenticacaoApplication.RevogarTokenAsync(request.RefreshToken);
        return Ok(new { mensagem = "Logout realizado com sucesso." });
    }
}
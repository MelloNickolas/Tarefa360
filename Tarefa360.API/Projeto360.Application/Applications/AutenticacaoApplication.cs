using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Projeto360.Application.Interfaces;
using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;
using Projeto360.Services.Interfaces;

namespace Projeto360.Application;

public class AutenticacaoApplication : IAutenticacaoApplication
{
    // Injeta os repositórios e serviços que já foram criados
    private readonly IUsuarioRepository    _usuarioRepository;
    private readonly ITwoFactorRepository  _twoFactorRepository;
    private readonly IEmailService         _emailService;         // vem do Passo 2
    private readonly IConfiguration        _config;

    public AutenticacaoApplication(
        IUsuarioRepository   usuarioRepository,
        ITwoFactorRepository twoFactorRepository,
        IEmailService        emailService,
        IConfiguration       config)
    {
        _usuarioRepository   = usuarioRepository;
        _twoFactorRepository = twoFactorRepository;
        _emailService        = emailService;
        _config              = config;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PASSO 1 DO LOGIN
    // Recebe e-mail + senha + token do captcha
    // Valida os três, gera o código 2FA e envia por e-mail
    // ─────────────────────────────────────────────────────────────────────────
    public async Task IniciarLoginAsync(string email, string senha, string captchaToken)
    {
        // 1a. Verifica o captcha chamando a API do Google
        //     Se o usuário não marcou o "Não sou robô", rejeita aqui
        var captchaValido = await VerificarCaptchaAsync(captchaToken);
        if (!captchaValido)
            throw new UnauthorizedAccessException("Verificação de CAPTCHA falhou. Tente novamente.");

        // 1b. Busca o usuário pelo e-mail e valida a senha com BCrypt
        //     (ObterPorEmailESenhaAsync já faz o BCrypt.Verify internamente)
        var usuario = await _usuarioRepository.ObterPorEmailESenhaAsync(email, senha);
        if (usuario == null)
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        // 1c. Cancela qualquer código anterior que ainda não tenha sido usado
        //     Isso evita que o usuário acumule códigos válidos no banco
        await _twoFactorRepository.InvalidarTokensAnterioresAsync(usuario.ID);

        // 1d. Gera um código de 6 dígitos usando gerador criptográfico seguro
        //     (não usa Random.Next — é mais seguro para fins de segurança)
        var codigo = GerarCodigo6Digitos();

        // 1e. Persiste o código no banco com validade de 10 minutos
        var token = new TwoFactorToken
        {
            UsuarioID = usuario.ID,
            Codigo    = codigo,
            Expiracao = DateTime.UtcNow.AddMinutes(10),
            Utilizado = false
        };
        await _twoFactorRepository.CriarAsync(token);

        // 1f. Chama o EmailService para enviar o código para o usuário
        await _emailService.EnviarCodigoVerificacaoAsync(usuario.Email, usuario.Nome, codigo);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PASSO 2 DO LOGIN
    // Recebe o e-mail e o código que o usuário digitou
    // Valida o código e devolve AccessToken (curto) + RefreshToken (longo)
    // ─────────────────────────────────────────────────────────────────────────
    public async Task<(string AccessToken, string RefreshToken)> ConfirmarCodigoDoisFatoresAsync(
        string email, string codigo)
    {
        // 2a. Localiza o usuário pelo e-mail
        var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
        if (usuario == null)
            throw new UnauthorizedAccessException("Usuário não encontrado.");

        // 2b. Busca no banco um token que:
        //     - pertença a este usuário
        //     - tenha o código correto
        //     - não tenha expirado
        //     - ainda não tenha sido utilizado
        var tokenValido = await _twoFactorRepository.ObterTokenValidoAsync(usuario.ID, codigo);
        if (tokenValido == null)
            throw new UnauthorizedAccessException("Código inválido ou expirado.");

        // 2c. Marca o código como usado para impedir que seja reutilizado
        await _twoFactorRepository.MarcarComoUtilizadoAsync(tokenValido);

        // 2d. Gera o AccessToken — JWT com claims do usuário, dura 60 minutos
        var accessToken  = GerarAccessToken(usuario);

        // 2e. Gera o RefreshToken — string aleatória longa, sem dados sensíveis
        var refreshToken = GerarRefreshToken();

        // 2f. Salva o RefreshToken no banco vinculado ao usuário (dura 7 dias)
        usuario.RefreshToken          = refreshToken;
        usuario.RefreshTokenExpiracao = DateTime.UtcNow.AddDays(7);
        await _usuarioRepository.AtualizarAsync(usuario);

        return (accessToken, refreshToken);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // RENOVAÇÃO AUTOMÁTICA DE TOKEN
    // O frontend chama este endpoint quando recebe um erro 401
    // Se o RefreshToken ainda for válido, devolve um novo par de tokens
    // ─────────────────────────────────────────────────────────────────────────
    public async Task<(string AccessToken, string RefreshToken)> RenovarTokenAsync(
        string refreshToken)
    {
        // Busca o usuário que possui exatamente este RefreshToken no banco
        var usuario = await _usuarioRepository.ObterPorRefreshTokenAsync(refreshToken);

        // Rejeita se o token não existe, se pertence a usuário inativo
        // ou se já passou dos 7 dias de validade
        if (usuario == null
            || usuario.RefreshTokenExpiracao == null
            || usuario.RefreshTokenExpiracao < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException(
                "Sessão expirada. Por favor, faça login novamente.");
        }

        // Gera novos tokens — a rotação do RefreshToken é uma prática de segurança:
        // cada renovação gera um token diferente, invalidando o anterior
        var novoAccessToken  = GerarAccessToken(usuario);
        var novoRefreshToken = GerarRefreshToken();

        // Atualiza o banco com o novo RefreshToken e estende a validade por mais 7 dias
        usuario.RefreshToken          = novoRefreshToken;
        usuario.RefreshTokenExpiracao = DateTime.UtcNow.AddDays(7);
        await _usuarioRepository.AtualizarAsync(usuario);

        return (novoAccessToken, novoRefreshToken);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // LOGOUT
    // Apaga o RefreshToken do banco — a sessão fica completamente inválida
    // ─────────────────────────────────────────────────────────────────────────
    public async Task RevogarTokenAsync(string refreshToken)
    {
        var usuario = await _usuarioRepository.ObterPorRefreshTokenAsync(refreshToken);

        // Se não encontrou, o token já foi revogado — sem erro, sem problema
        if (usuario == null) return;

        // Limpa os campos — próxima tentativa de renovação será rejeitada
        usuario.RefreshToken          = null;
        usuario.RefreshTokenExpiracao = null;
        await _usuarioRepository.AtualizarAsync(usuario);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // MÉTODOS PRIVADOS
    // ─────────────────────────────────────────────────────────────────────────

    // Consulta a API do Google para verificar se o token do reCAPTCHA é legítimo
    private async Task<bool> VerificarCaptchaAsync(string captchaToken)
    {
        // Lê a SecretKey do appsettings.json — nunca exposta no frontend
        var secretKey = _config["Recaptcha:SecretKey"];

        using var http = new HttpClient();

        // A API do Google espera um POST com form-data (não JSON)
        var resposta = await http.PostAsync(
            "https://www.google.com/recaptcha/api/siteverify",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "secret",   secretKey! },
                { "response", captchaToken }
            })
        );

        var json = await resposta.Content.ReadAsStringAsync();

        // A resposta tem o formato: { "success": true, "challenge_ts": "...", ... }
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("success").GetBoolean();
    }

    // Gera o JWT (AccessToken) com as informações básicas do usuário como claims
    private string GerarAccessToken(Usuario usuario)
    {
        var key     = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
        var minutos = int.TryParse(_config["Jwt:AccessTokenMinutos"], out var m) ? m : 60;

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name,  usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("Id",             usuario.ID.ToString()),
                // Role permite que [Authorize(Roles = "Administrador")] funcione
                new Claim(ClaimTypes.Role,  usuario.TipoUsuario.ToString())
            }),
            Expires            = DateTime.UtcNow.AddMinutes(minutos),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }

    // Gera uma string aleatória de 64 bytes em Base64 — sem nenhum dado do usuário
    private static string GerarRefreshToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    // Gera código numérico de 6 dígitos usando criptografia (mais seguro que Random)
    private static string GerarCodigo6Digitos()
    {
        var bytes = new byte[4];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        // % 1_000_000 garante que fica entre 0 e 999999
        // "D6" formata com zeros à esquerda: ex: 007423
        var numero = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 1_000_000;
        return numero.ToString("D6");
    }
}
namespace Projeto360.Services.Interfaces;

public interface IEmailService
{
    // Envia o código de verificação 2FA para o e-mail do usuário
    Task EnviarCodigoVerificacaoAsync(string destinatario, string nomeUsuario, string codigo);
}
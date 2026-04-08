using System.Net;
using System.Net.Mail;
using Projeto360.Services.Interfaces;
using Microsoft.Extensions.Configuration;

// Mesma pasta/namespace do padrão já usado em JsonPlaceHolderService
namespace Projeto360.Services.EmailService;

public class EmailService : IEmailService
{
    // IConfiguration injeta automaticamente os valores do appsettings.json
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task EnviarCodigoVerificacaoAsync(
        string destinatario, string nomeUsuario, string codigo)
    {
        // Lê as configurações já preenchidas no seu appsettings.json
        var smtpHost      = _config["Email:SmtpHost"];       // smtp.gmail.com
        var smtpPort      = int.Parse(_config["Email:SmtpPort"] ?? "587");
        var smtpUser      = _config["Email:SmtpUser"];       // admin.tarefa360@gmail.com
        var smtpPass      = _config["Email:SmtpPassword"];   // ocjo ypmk gtua irpw
        var remetente     = _config["Email:Remetente"];
        var nomeRemetente = _config["Email:NomeRemetente"] ?? "Tarefa360";

        // Corpo do e-mail em HTML com a cor laranja do sistema (#eb8d3e)
        var corpo = $@"
            <div style='font-family:Arial,sans-serif;max-width:480px;
                        margin:0 auto;padding:32px;border:1px solid #e0e0e0;
                        border-radius:8px;'>

              <h2 style='color:#eb8d3e;margin-bottom:8px;'>Verificação de Acesso</h2>

              <p>Olá, <strong>{nomeUsuario}</strong>!</p>
              <p>Seu código de verificação para acessar o <strong>Tarefa360</strong> é:</p>

              <div style='font-size:36px;font-weight:bold;letter-spacing:10px;
                          color:#eb8d3e;text-align:center;padding:20px 0;'>
                {codigo}
              </div>

              <p style='color:#888;font-size:13px;'>
                Este código expira em <strong>10 minutos</strong>.
                Não compartilhe com ninguém.
              </p>

              <hr style='border:none;border-top:1px solid #eee;margin:24px 0;'/>

              <p style='color:#aaa;font-size:12px;'>
                Se você não tentou fazer login, ignore este e-mail.
              </p>

            </div>";

        // SmtpClient usa as configurações lidas acima para conectar ao Gmail
        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            // NetworkCredential usa o e-mail e a Senha de App do Gmail
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl   = true  // Gmail exige conexão segura
        };

        var mensagem = new MailMessage
        {
            From       = new MailAddress(remetente!, nomeRemetente),
            Subject    = "Código de verificação — Tarefa360",
            Body       = corpo,
            IsBodyHtml = true   // Ativa a renderização do HTML no e-mail
        };

        // Destinatário é o e-mail do usuário que está tentando logar
        mensagem.To.Add(destinatario);

        await client.SendMailAsync(mensagem);
    }
}
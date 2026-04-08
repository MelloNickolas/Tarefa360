namespace Projeto360.Domain.Entities;
public class TwoFactorToken
{
    public int ID { get; set; }
    // Qual usuário está tentando logar
    public int UsuarioID { get; set; }
    // O código de 6 dígitos enviado por e-mail (ex: "482931")
    public string Codigo { get; set; }
    // Quando o código expira (criação + 10 minutos)
    public DateTime Expiracao { get; set; }
    // Evita que o mesmo código seja usado duas vezes
    public bool Utilizado { get; set; }
    // Navegação para o usuário dono do token
    public Usuario Usuario { get; set; }
    // Método auxiliar para checar se o código ainda é válido
    public bool EstaValido() =>
    !Utilizado && DateTime.UtcNow <= Expiracao;
}
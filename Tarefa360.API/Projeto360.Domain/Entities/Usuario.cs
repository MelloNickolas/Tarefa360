using Projeto360.Domain.Enums;

namespace Projeto360.Domain.Entities;

public class Usuario
{
    public int ID { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public bool Ativo { get; set; }
    public TiposUsuario TipoUsuario { get; set; }

    // Campo de relacionamento muitos para Tarefa
    // Collection é um List mais flexível da uma pesquisada sobre!
    public ICollection<Tarefa> Tarefas { get; set; }

    // Campo para relacionamento muitos do UsuarioEquipe
    public ICollection<UsuarioEquipe> UsuarioEquipes { get; set; }

    // NOVO: Campos para o Refresh Token
    // Nullable (?) porque usuários já cadastrados não têm esses valores
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiracao { get; set; }

    // NOVO: Relacionamento com os tokens de 2FA
    public ICollection<TwoFactorToken> TwoFactorTokens { get; set; }

    public Usuario()
    {
        Ativo = true;
    }

    public void Deletar()
    {
        Ativo = false;
    }

    public void Restaurar()
    {
        Ativo = true;
    }
}
using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Interfaces;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> ListarAsync(bool ativo);
    Task<int> CriarAsync(Usuario usuario);
    Task<Usuario> ObterPorIdAsync(int id);
    Task<Usuario> ObterPorEmailAsync(string email);
    Task AtualizarAsync(Usuario usuario);
    Task DeletarAsync(Usuario usuario);
    Task<Usuario> ObterPorEmailESenhaAsync(string email, string senha);

    // NOVO: Busca usuário pelo RefreshToken para validar renovação
    Task<Usuario> ObterPorRefreshTokenAsync(string refreshToken);

}
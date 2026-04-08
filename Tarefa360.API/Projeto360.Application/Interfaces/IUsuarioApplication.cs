using Projeto360.Domain.Entities;
using Projeto360.Domain.Enums;

namespace Projeto360.Application.Interfaces;
public interface IUsuarioApplication
{
    Task<int> CriarAsync(Usuario usuarioDTO);
    Task AtualizarAsync(Usuario usuarioDTO);
    Task AtualizarSenhaAsync(Usuario usuarioDTO, string senhaAtual);
    Task<Usuario> ObterPorIdAsync(int usuarioID);
    Task<Usuario> ObterPorEmailAsync(string email);
    Task DeletarAsync(int usuarioID);
    Task RestaurarAsync(int usuarioID);
    Task<IEnumerable<Usuario>> ListarAsync(bool ativo);
    Dictionary<TiposUsuario, string> ListarTiposUsuarioAsync();
    Task<Usuario> ObterPorEmailESenhaAsync(string email, string senha);
}
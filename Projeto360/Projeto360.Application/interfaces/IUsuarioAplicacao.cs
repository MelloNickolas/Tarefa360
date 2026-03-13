using Projeto360.Dominio.Entidades;

namespace Projeto360.Application
{
  public interface IUsuarioAplicacao
  {
    int Criar(Usuario usuarioDTO);
    void AtualizarSenha(Usuario usuarioDTO, string senhaAntiga);
    void Atualizar(Usuario usuarioDTO);
    void Deletar(int IdUsuario);
    void Restaurar(int IdUsuario);
    IEnumerable<Usuario> Listar(bool ativo);
    Usuario Obter(int IdUsuario);
    Usuario ObterPorEmail(string email);




    Task<int> CriarAsync(Usuario usuarioDTO);
    Task AtualizarSenhaAsync(Usuario usuarioDTO, string senhaAntiga);
    Task AtualizarAsync(Usuario usuarioDTO);
    Task DeletarAsync(int IdUsuario);
    Task RestaurarAsync(int IdUsuario);
    Task<IEnumerable<Usuario>> ListarAsync(bool ativo);
    Task<Usuario> ObterAsync(int IdUsuario);
    Task<Usuario> ObterPorEmailAsync(string email);
  }
}
using Projeto360.Dominio.Entidades;

namespace Projeto360.Application
{
  public class UsuarioAplicacao : IUsuarioAplicacao
  {
    readonly IUsuarioRepository _usuarioRepository;

    public UsuarioAplicacao(IUsuarioRepository usuarioRepository)
    {
      _usuarioRepository = usuarioRepository;
    }

    public int Criar(Usuario usuario)
    {
      if (usuario == null)
        throw new Exception("Não pode ter um Usuário vazio!");
      ValidarInformacoesUsuario(usuario);
      if (usuario.Senha == null)
        throw new Exception("Não pode ter uma senha do Usuário vazio!");

      return _usuarioRepository.Salvar(usuario);
    }

    public void Atualizar(Usuario usuario)
    {
      var usuarioDominio = _usuarioRepository.Obter(usuario.IDUsuario);
      if (usuarioDominio == null)
        throw new Exception("Usuario não encontrado!");
      ValidarInformacoesUsuario(usuario);

      usuarioDominio.Nome = usuario.Nome;
      usuarioDominio.Email = usuario.Email;

      _usuarioRepository.Atualizar(usuarioDominio);
    }

    public void AtualizarSenha(Usuario usuario, string senhaAntiga)
    {
      var usuarioDominio = _usuarioRepository.Obter(usuario.IDUsuario);
      if (usuarioDominio == null)
        throw new Exception("Usuario não encontrado!");
      ValidarInformacoesUsuario(usuario);

      if (senhaAntiga != usuarioDominio.Senha)
        throw new Exception("Senha Inválida");

      usuarioDominio.Senha = usuario.Senha;

      _usuarioRepository.Atualizar(usuarioDominio);
    }

    public Usuario Obter(int IdUsuario)
    {
      var usuarioDominio = _usuarioRepository.Obter(IdUsuario);
      if (usuarioDominio == null)
        throw new Exception("Usuario não encontrado!");

      return usuarioDominio;
    }

    public Usuario ObterPorEmail(string email)
    {
      var usuarioDominio = _usuarioRepository.ObterPorEmail(email);
      if (usuarioDominio == null)
        throw new Exception("Usuario não encontrado!");

      return usuarioDominio;
    }

    public void Deletar(int IdUsuario)
    {
      var usuarioDominio = _usuarioRepository.Obter(IdUsuario);
      if (usuarioDominio == null)
        throw new Exception("Usuario não encontrado!");

      usuarioDominio.Deletar();

      _usuarioRepository.Atualizar(usuarioDominio);
    }

    public void Restaurar(int IdUsuario)
    {
      var usuarioDominio = _usuarioRepository.Obter(IdUsuario);
      if (usuarioDominio == null)
        throw new Exception("Usuario não encontrado!");

      usuarioDominio.Recuperar();

      _usuarioRepository.Atualizar(usuarioDominio);
    }

    public IEnumerable<Usuario> Listar(bool ativo)
    {
      return _usuarioRepository.Listar(ativo);
    }










    public async Task<int> CriarAsync(Usuario usuario)
    {
      if (usuario == null)
        throw new Exception("Não pode ter um Usuário vazio!");
      ValidarInformacoesUsuario(usuario);
      if (usuario.Senha == null)
        throw new Exception("Não pode ter uma senha do Usuário vazio!");

      return await _usuarioRepository.SalvarAsync(usuario);
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
      var usuarioDominio = await _usuarioRepository.ObterAsync(usuario.IDUsuario);
      if (usuarioDominio == null)
        throw new Exception("Usuario não encontrado!");
      ValidarInformacoesUsuario(usuario);

      usuarioDominio.Nome = usuario.Nome;
      usuarioDominio.Email = usuario.Email;

      await _usuarioRepository.AtualizarAsync(usuarioDominio);
    }

    public async Task AtualizarSenhaAsync(Usuario usuario, string senhaAntiga)
    {
      var usuarioDominio = await _usuarioRepository.ObterAsync(usuario.IDUsuario);
      if (usuarioDominio == null)
        throw new Exception("Usuario não encontrado!");
      ValidarInformacoesUsuario(usuario);

      if (senhaAntiga != usuarioDominio.Senha)
        throw new Exception("Senha Inválida");

      usuarioDominio.Senha = usuario.Senha;

      await _usuarioRepository.AtualizarAsync(usuarioDominio);
    }

    public async Task<Usuario> ObterAsync(int IdUsuario)
    {
      var usuarioDominio = await _usuarioRepository.ObterAsync(IdUsuario);
      if (usuarioDominio == null)
        throw new Exception("Usuario não encontrado!");

      return usuarioDominio;
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
      var usuarioDominio = await _usuarioRepository.ObterPorEmailAsync(email);
      if (usuarioDominio == null)
        throw new Exception("Usuario não encontrado!");

      return usuarioDominio;
    }

    public async Task DeletarAsync(int IdUsuario)
    {
      var usuarioDominio = await _usuarioRepository.ObterAsync(IdUsuario);
      if (usuarioDominio == null)
        throw new Exception("Usuario não encontrado!");

      usuarioDominio.Deletar();

      await _usuarioRepository.AtualizarAsync(usuarioDominio);
    }

    public async Task RestaurarAsync(int IdUsuario)
    {
      var usuarioDominio = await _usuarioRepository.ObterAsync(IdUsuario);
      if (usuarioDominio == null)
        throw new Exception("Usuario não encontrado!");

      usuarioDominio.Recuperar();

      await _usuarioRepository.AtualizarAsync(usuarioDominio);
    }

    public async Task<IEnumerable<Usuario>> ListarAsync(bool ativo)
    {
      return await _usuarioRepository.ListarAsync(ativo);
    }








    #region Ultilidades
    private static void ValidarInformacoesUsuario(Usuario usuario)
    {
      if (String.IsNullOrEmpty(usuario.Nome))
        throw new Exception("Não pode ter um nome do Usuário vazio!");
      if (String.IsNullOrEmpty(usuario.Email))
        throw new Exception("Não pode ter um email do Usuário vazio!");
    }
    #endregion
  }
}
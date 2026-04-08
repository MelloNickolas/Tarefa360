using Projeto360.Domain.Entities;
using Projeto360.Domain.Enums;
using Projeto360.Repository.Interfaces;
using Projeto360.Application.Interfaces;

namespace Projeto360.Application;

public class UsuarioApplication : IUsuarioApplication
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioApplication(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<int> CriarAsync(Usuario usuario)
    {
        if (usuario == null)
            throw new Exception("Usuário não pode ser vazio.");
        ValidarInformacoesUsuario(usuario);

        var usuarioExistente = await _usuarioRepository.ObterPorEmailAsync(usuario.Email);
        if (usuarioExistente != null)
            throw new Exception("Já existe usuário com o E-mail informado.");

        if (string.IsNullOrWhiteSpace(usuario.Senha))
            throw new Exception("Senha não pode ser vazia.");

        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
        
        return await _usuarioRepository.CriarAsync(usuario);
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        Usuario usuarioExistenteId = await ValidarUsuarioExistentePorId(usuario.ID);

        var usuarioExistenteEmail = await _usuarioRepository.ObterPorEmailAsync(usuario.Email);
        if (usuarioExistenteEmail != null && usuarioExistenteEmail.ID != usuarioExistenteId.ID)
            throw new Exception("Já existe usuário com o E-mail informado.");

        ValidarInformacoesUsuario(usuario);

        usuarioExistenteId.Nome = usuario.Nome;
        usuarioExistenteId.Email = usuario.Email;
        usuarioExistenteId.TipoUsuario = usuario.TipoUsuario;

        await _usuarioRepository.AtualizarAsync(usuarioExistenteId);
    }

    public async Task AtualizarSenhaAsync(Usuario usuario, string senhaAtual)
    {
        Usuario usuarioExistente = await ValidarUsuarioExistentePorId(usuario.ID);

        if (usuarioExistente.Senha != senhaAtual)
            throw new Exception("Senha Atual incorreta.");

        usuarioExistente.Senha = usuario.Senha;

        await _usuarioRepository.AtualizarAsync(usuarioExistente);
    }

    public async Task<Usuario> ObterPorIdAsync(int usuarioID)
    {
        Usuario usuarioExistente = await ValidarUsuarioExistentePorId(usuarioID);

        return usuarioExistente;
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        var usuarioExistente = await _usuarioRepository.ObterPorEmailAsync(email);
        if (usuarioExistente == null)
            throw new Exception("Usuário não localizado.");

        return usuarioExistente;
    }

    public async Task DeletarAsync(int usuarioID)
    {
        Usuario usuarioExistente = await ValidarUsuarioExistentePorId(usuarioID);

        usuarioExistente.Deletar();

        await _usuarioRepository.AtualizarAsync(usuarioExistente);
    }

    public async Task RestaurarAsync(int usuarioId)
    {
        Usuario usuarioExistente = await ValidarUsuarioExistentePorId(usuarioId);

        usuarioExistente.Restaurar();

        await _usuarioRepository.AtualizarAsync(usuarioExistente);
    }

    public async Task<IEnumerable<Usuario>> ListarAsync(bool ativo)
    {
        return await _usuarioRepository.ListarAsync(ativo);
    }

    public Dictionary<TiposUsuario, string> ListarTiposUsuarioAsync()
    {
        // Cria um dicionário para armazenar os tipos de usuário e suas descrições
        var tiposUsuario = new Dictionary<TiposUsuario, string>();

        // Percorre os valores do enum TiposUsuario e adiciona ao dicionário
        foreach (TiposUsuario tipo in Enum.GetValues(typeof(TiposUsuario)))
        {
            tiposUsuario[tipo] = tipo.ToString();
        }
        return tiposUsuario;
    }

    public Task<Usuario> ObterPorEmailESenhaAsync(string email, string senha)
    {
        return _usuarioRepository.ObterPorEmailESenhaAsync(email, senha);
    }

    #region Úteis
    private static void ValidarInformacoesUsuario(Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario.Nome))
            throw new Exception("Nome não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(usuario.Email))
            throw new Exception("E-mail não pode ser vazio.");
        
        if (!Enum.IsDefined(typeof(TiposUsuario), usuario.TipoUsuario))
            throw new Exception("Tipo de usuário inválido.");
    }

    private async Task<Usuario> ValidarUsuarioExistentePorId(int usuarioId)
    {
        var usuarioExistente = await _usuarioRepository.ObterPorIdAsync(usuarioId);
        if (usuarioExistente == null)
            throw new Exception("Usuário não localizado.");
        return usuarioExistente;
    }

    #endregion
}
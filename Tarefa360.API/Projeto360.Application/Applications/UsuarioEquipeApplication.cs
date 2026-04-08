using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;
using Projeto360.Application.Interfaces;
using Projeto360.Domain.Enums;

namespace Projeto360.Application;

public class UsuarioEquipeApplication : IUsuarioEquipeApplication
{
  private readonly IUsuarioEquipeRepository _usuarioEquipeRepository;
  private readonly IEquipeRepository _equipeRepository;
  private readonly IUsuarioRepository _usuarioRepository;
  public UsuarioEquipeApplication(
    IUsuarioEquipeRepository usuarioEquipeRepository,
    IEquipeRepository equipeRepository,
    IUsuarioRepository usuarioRepository
    )
  {
    _usuarioEquipeRepository = usuarioEquipeRepository;
    _equipeRepository = equipeRepository;
    _usuarioRepository = usuarioRepository;
  }

  public async Task<UsuarioEquipe> AtualizarPapelPorIdAsync(UsuarioEquipe usuarioEquipe)
  {
    if (!Enum.IsDefined(typeof(PapeisEquipe), usuarioEquipe.PapeisEquipe))
      throw new Exception("O PAPEL informado é inválido.");

    var existente = await _usuarioEquipeRepository.ObterPorIdAsync(usuarioEquipe.ID);
    if (existente == null)
      throw new Exception("Registro não encontrado.");

    existente.PapeisEquipe = usuarioEquipe.PapeisEquipe;

    return await _usuarioEquipeRepository.AtualizarPapelPorIdAsync(existente);
  }

  public async Task<int> CriarAsync(UsuarioEquipe usuarioEquipe)
  {
    var verificaEquipeId = await _equipeRepository.ObterEquipePorIdAsync(usuarioEquipe.EquipeId);
    if (verificaEquipeId == null)
      throw new Exception("O ID da EQUIPE não existe");

    var verificaUsuarioId = await _usuarioRepository.ObterPorIdAsync(usuarioEquipe.UsuarioId);
    if (verificaUsuarioId == null)
      throw new Exception("O ID do USUÁRIO não existe");

    if (!Enum.IsDefined(typeof(PapeisEquipe), usuarioEquipe.PapeisEquipe))
      throw new Exception("O PAPEL informado é inválido.");


    await _usuarioEquipeRepository.CriarAsync(usuarioEquipe);
    return usuarioEquipe.ID;
  }

  public async Task DeletarAsync(UsuarioEquipe usuarioEquipe)
  {
    var relacionamentoDeletar = await ObterPorIdAsync(usuarioEquipe.ID);
    await _usuarioEquipeRepository.DeletarAsync(relacionamentoDeletar);
  }

  public Dictionary<PapeisEquipe, string> ListarPapeisEquipeAsync()
  {
    // Cria um dicionário para armazenar os tipos de usuário e suas descrições
    var papeisEquipe = new Dictionary<PapeisEquipe, string>();

    // Percorre os valores do enum TiposUsuario e adiciona ao dicionário
    foreach (PapeisEquipe papel in Enum.GetValues(typeof(PapeisEquipe)))
    {
      papeisEquipe[papel] = papel.ToString();
    }
    return papeisEquipe;
  }

  public async Task<IEnumerable<UsuarioEquipe>> ListarPorEquipesAsync(int equipeID)
  {
    var verificaEquipeId = await _equipeRepository.ObterEquipePorIdAsync(equipeID);
    if (verificaEquipeId == null)
      throw new Exception("O ID da EQUIPE não existe");

    return await _usuarioEquipeRepository.ListarPorEquipesAsync(verificaEquipeId.ID);
  }

  public async Task<IEnumerable<UsuarioEquipe>> ListarPorUsuariosAsync(int usuarioID)
  {
    var verificaUsuarioId = await _usuarioRepository.ObterPorIdAsync(usuarioID);
    if (verificaUsuarioId == null)
      throw new Exception("O ID do USUÁRIO não existe");

    return await _usuarioEquipeRepository.ListarPorUsuariosAsync(verificaUsuarioId.ID);
  }

  public async Task<UsuarioEquipe> ObterPorIdAsync(int id)
  {
    var relacionamentoId = await _usuarioEquipeRepository.ObterPorIdAsync(id);
    if (relacionamentoId == null)
      throw new Exception("O ID do relacionamento não existe");

    return relacionamentoId;
  }


}
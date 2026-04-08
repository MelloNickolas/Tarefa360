using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;
using Projeto360.Application.Interfaces;

namespace Projeto360.Application;

public class EquipeApplication : IEquipeApplication
{
  private readonly IEquipeRepository _equipeRepository;

  public EquipeApplication(IEquipeRepository equipeRepository)
  {
    _equipeRepository = equipeRepository;
  }

  public async Task<int> CriarEquipeAsync(Equipe equipe)
  {
    if (equipe == null)
      throw new Exception("Equipe não pode ser vazia");
    ValidarInformacoesEquipe(equipe);

    var projetoExistente = await _equipeRepository.ObterEquipePorNomeExatoAsync(equipe.Nome);
    if (projetoExistente.Any())
      throw new Exception("Já existe uma equipe com o nome informado.");

    return await _equipeRepository.CriarEquipeAsync(equipe);
  }

  public async Task AtualizarEquipeAsync(Equipe equipe)
  {
    Equipe equipeExistenteId = await ValidarEquipeExistentePorId(equipe.ID);

    var equipeExistenteNome = await _equipeRepository.ObterEquipePorNomeAsync(equipe.Nome);
    if (equipeExistenteNome.Any(e => e.ID != equipe.ID))
      throw new Exception("Já existe equipe com o nome informado.");
    ValidarInformacoesEquipe(equipe);

    equipeExistenteId.Nome = equipe.Nome;

    await _equipeRepository.AtualizarEquipeAsync(equipeExistenteId);
  }

  public async Task<Equipe> ObterEquipePorIdAsync(int equipeID)
  {
    Equipe equipeExistente = await ValidarEquipeExistentePorId(equipeID);

    return equipeExistente;
  }

  public async Task<IEnumerable<Equipe>> ObterEquipePorNomeAsync(string nome)
  {
    var equipeExistente = await _equipeRepository.ObterEquipePorNomeAsync(nome);
    if (equipeExistente == null)
      throw new Exception("Equipe não localizada.");

    return equipeExistente;
  }

  public async Task DeletarEquipeAsync(int equipeID)
  {
    Equipe equipeExistente = await ValidarEquipeExistentePorId(equipeID);

    await _equipeRepository.DeletarEquipeAsync(equipeExistente);
  }

  public async Task<IEnumerable<Equipe>> ListarEquipeAsync()
  {
    return await _equipeRepository.ListarEquipesAsync();
  }

  public async Task<IEnumerable<Equipe>> ListarComMembrosAsync()
  {
    return await _equipeRepository.ListarMembrosPorEquipeAsync();
  }


  #region Úteis
  private static void ValidarInformacoesEquipe(Equipe equipe)
  {
    if (string.IsNullOrWhiteSpace(equipe.Nome))
      throw new Exception("Nome da equipe não pode ser vazio.");
  }

  private async Task<Equipe> ValidarEquipeExistentePorId(int equipeId)
  {
    var equipeExistente = await _equipeRepository.ObterEquipePorIdAsync(equipeId);
    if (equipeExistente == null)
      throw new Exception("Equipe não localizada.");
    return equipeExistente;
  }
  #endregion
}
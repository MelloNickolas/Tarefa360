using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;
using Projeto360.Application.Interfaces;
#nullable enable

namespace Projeto360.Application;

public class SprintApplication : ISprintApplication
{
  private readonly ISprintRepository _sprintRepository;
  private readonly IProjetoRepository _projetoRepository;

  public SprintApplication(ISprintRepository sprintRepository, IProjetoRepository projetoRepository)
  {
    _sprintRepository = sprintRepository;
    _projetoRepository = projetoRepository;
  }

  public async Task<Sprint?> ObterPorIdAsync(int id)
  {
    var sprintExistente = await _sprintRepository.ObterPorIdAsync(id);
    if (sprintExistente == null)
      throw new Exception("Não existe nenhuma SPRINT com esse ID");

    return sprintExistente;
  }

  public async Task<bool> AtualizarAsync(Sprint sprint)
  {
    await ObterPorIdAsync(sprint.ID);
    ValidarSprint(sprint);
    await _sprintRepository.AtualizarAsync(sprint);
    return true;
  }

  public async Task<bool> DeletarAsync(int id)
  {
    var sprint = await ObterPorIdAsync(id);
    await _sprintRepository.DeletarAsync(sprint!);
    return true;
  }

  public async Task<IEnumerable<Sprint>> ListarAsync()
  {
    return await _sprintRepository.ListarAsync();
  }

  public async Task<IEnumerable<Sprint>> ListarPorProjetoAsync(int projetoId)
  {
    var projeto = await _projetoRepository.ObterProjetoPorIdAsync(projetoId);
    if (projeto == null)
      throw new Exception("Esse PROJETO não existe!");

    return await _sprintRepository.ListarPorProjetoAsync(projetoId);
  }

  public async Task<IEnumerable<Sprint>> ObterPorTituloAsync(string titulo)
  {
    var sprint = await _sprintRepository.ObterPorTituloAsync(titulo);
    if (sprint == null || !sprint.Any())
      throw new Exception("Não existe nenhuma SPRINT com esse TÍTULO!");

    return sprint;
  }

  public async Task<int> CriarAsync(Sprint sprint)
  {
    ValidarSprint(sprint);
    await _sprintRepository.SalvarAsync(sprint);
    return sprint.ID;
  }

  #region Métodos para validações
  public Sprint ValidarSprint(Sprint sprint)
  {
    if (string.IsNullOrWhiteSpace(sprint.Titulo) || sprint.Titulo.Length < 3 || sprint.Titulo.Length > 100)
      throw new Exception("O TÍTULO deve ter entre 3 e 100 caracteres e não pode ser nulo");

    if (sprint.ProjetoID == 0)
      throw new Exception("A SPRINT deve pertencer a algum PROJETO");

    return sprint;
  }
  #endregion
}
using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;
using Projeto360.Application.Interfaces;

namespace Projeto360.Application;

public class HistoriaApplication : IHistoriaApplication
{
  private readonly IHistoriaRepository _historiaRepository;
  
  /* Injeção de Dependecia para podermos validar se o Id do projeto realmente existe para listarmos */
  private readonly IProjetoRepository _projetoRepository;
  
  public HistoriaApplication(IHistoriaRepository historiaRepository, IProjetoRepository projetoRepository)
  {
    _historiaRepository = historiaRepository;
    _projetoRepository = projetoRepository;
  }


  public async Task<Historia> ObterPorIdAsync(int id)
  {
    var historiaExistenteId = await _historiaRepository.ObterPorIdAsync(id);
    if (historiaExistenteId == null)
      throw new Exception("Não existe nenhuma HISTÓRIA com esse ID");

    return historiaExistenteId;
  }

  public async Task AtualizarAsync(Historia historia)
  { 
    await ObterPorIdAsync(historia.ID);    
    ValidarHistoria(historia);
    await _historiaRepository.AtualizarAsync(historia);
  }

  public async Task DeletarAsync(int id)
  {
    var deletarHistoria = await ObterPorIdAsync(id);
    await _historiaRepository.DeletarAsync(deletarHistoria);
  }

  public async Task<IEnumerable<Historia>> ListarAsync()
  {
    return await _historiaRepository.ListarAsync();
  }

  public async Task<IEnumerable<Historia>> ListarPorProjetoAsync(int projetoId)
  {
    var verificaProjetoExistente = await _projetoRepository.ObterProjetoPorIdAsync(projetoId);
    if(verificaProjetoExistente == null)
      throw new Exception("Esse PROJETO não existe!");

    return await _historiaRepository.ListarPorProjetoAsync(projetoId);
  }


  public async Task<IEnumerable<Historia>> ObterPorNomeAsync(string nome)
  {
    var verificarNomeExistente = await _historiaRepository.ObterPorNomeAsync(nome);
    if(verificarNomeExistente == null)
      throw new Exception("Não possui nenhuma história com esse NOME!");

    return verificarNomeExistente;
  }

  public async Task<int> SalvarAsync(Historia historia)
  {
    ValidarHistoria(historia);
    await _historiaRepository.SalvarAsync(historia);

    return historia.ID;
  }



  #region Métodos para validações
  public Historia ValidarHistoria(Historia historia)
  {
      if (string.IsNullOrWhiteSpace(historia.Nome) || historia.Nome.Length < 3 || historia.Nome.Length > 100)
      throw new Exception("O NOME deve ter entre 3 e 100 caracteres e não pode ser nulo");

    if (historia.ProjetoID == 0)
      throw new Exception("A HISTÓRIA tem que pertencer a algum PROJETO");

    return historia;
  }
  #endregion
}
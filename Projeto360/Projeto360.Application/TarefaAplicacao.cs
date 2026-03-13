using Projeto360.Dominio.Entidades;
using Projeto360.Services.Interfaces;

namespace Projeto360.Application;

public class TarefaAplicacao : ITarefaAplicacao
{
  private readonly IJsonPlaceHolderService _jsonPlaceHolderService;

  public TarefaAplicacao(IJsonPlaceHolderService jsonPlaceHolderService)
  {
    _jsonPlaceHolderService = jsonPlaceHolderService;
  }

  public List<Tarefa> ListarTarefas()
  {
    return _jsonPlaceHolderService.ListarTarefas().Result; //result é para relacionar um async a um metodo normal
  }
}
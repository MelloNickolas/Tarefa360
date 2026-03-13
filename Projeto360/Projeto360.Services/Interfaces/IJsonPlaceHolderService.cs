using Projeto360.Dominio.Entidades;

namespace Projeto360.Services.Interfaces;
public interface IJsonPlaceHolderService
{
  Task<List<Tarefa>> ListarTarefas();
}
using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Interfaces;

public interface ITarefaRepository
{
    Task<IEnumerable<Tarefa>> ListarTodasAsync();
    Task<IEnumerable<Tarefa>> ListarAsync(bool concluida);
    Task<int> CriarAsync(Tarefa tarefa);
    Task<Tarefa> ObterPorIdAsync(int id);
    Task AtualizarAsync(Tarefa tarefa);
    Task DeletarAsync(Tarefa tarefa);
}
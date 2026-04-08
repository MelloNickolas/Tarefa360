using Projeto360.Domain.Entities;
using Projeto360.Domain.Enums;

namespace Projeto360.Application.Interfaces;
public interface ITarefaApplication
{
    Task<int> CriarAsync(Tarefa tarefaDTO);
    Task AtualizarAsync(Tarefa tarefaDTO);
    Task<Tarefa> ObterPorIdAsync(int tarefaID);
    Task DeletarAsync(int tarefaID);
    Task ConcluirTarefaAsync(int tarefaID);
    Task RetomarTarefaAsync(int tarefaID);
    Task<IEnumerable<Tarefa>> ListarAsync(bool concluida);
    Task<IEnumerable<Tarefa>> ListarTodasAsync();
    Task<Dictionary<TiposTarefa, string>> ListarTiposTarefaAsync();
}
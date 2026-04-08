using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Interfaces;

public interface ISprintRepository
{
    Task<int> SalvarAsync(Sprint sprint);
    Task AtualizarAsync(Sprint sprint);
    Task<Sprint> ObterPorIdAsync(int id);
    Task<IEnumerable<Sprint>> ListarAsync();
    Task DeletarAsync(Sprint sprint);

    // Buscar por título (nome da sprint)
    Task<IEnumerable<Sprint>> ObterPorTituloAsync(string titulo);

    // Listar sprints por projeto (relacionamento com ProjetoID)
    Task<IEnumerable<Sprint>> ListarPorProjetoAsync(int projetoId);
}

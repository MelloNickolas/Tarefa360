using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Interfaces;

public interface IProjetoRepository
{
    Task<IEnumerable<Projeto>> ListarProjetoAsync(bool ativo);
    Task<int> CriarProjetoAsync(Projeto projeto);
    Task<Projeto> ObterProjetoPorIdAsync(int id);
    Task AtualizarProjetoAsync(Projeto projeto);
    Task DeletarProjetoAsync(Projeto projeto);
    Task<Projeto> ObterProjetoPorNomeAsync(string nome);
}
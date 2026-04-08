using Projeto360.Domain.Entities;

namespace Projeto360.Application.Interfaces;
public interface IProjetoApplication
{
    Task<int> CriarProjetoAsync(Projeto projetoDTO);
    Task AtualizarProjetoAsync(Projeto projetoDTO);
    Task<Projeto> ObterProjetoPorIdAsync(int projetoID);
    Task<Projeto> ObterProjetoPorNomeAsync(string nome);
    Task DeletarProjetoAsync(int projetoID);
    Task RestaurarProjetoAsync(int projetoID);
    Task<IEnumerable<Projeto>> ListarProjetoAsync(bool ativo);
}
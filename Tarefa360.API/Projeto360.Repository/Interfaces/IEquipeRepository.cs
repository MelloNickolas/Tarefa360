using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Interfaces;

public interface IEquipeRepository
{
    Task<IEnumerable<Equipe>> ListarEquipesAsync();
    Task<int> CriarEquipeAsync(Equipe equipe);
    Task<Equipe> ObterEquipePorIdAsync(int id);
    Task AtualizarEquipeAsync(Equipe equipe);
    Task DeletarEquipeAsync(Equipe equipe);
    Task<IEnumerable<Equipe>> ObterEquipePorNomeAsync(string nome);
    Task<IEnumerable<Equipe>> ObterEquipePorNomeExatoAsync(string nome);
    Task<IEnumerable<Equipe>> ListarMembrosPorEquipeAsync();

}
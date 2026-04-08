using Projeto360.Domain.Entities;

namespace Projeto360.Application.Interfaces;

public interface IEquipeApplication
{
    Task<int> CriarEquipeAsync(Equipe equipeDTO);
    Task AtualizarEquipeAsync(Equipe equipeDTO);
    Task<Equipe> ObterEquipePorIdAsync(int equipeID);
    Task<IEnumerable<Equipe>> ObterEquipePorNomeAsync(string nome);
    Task DeletarEquipeAsync(int equipeID);
    Task<IEnumerable<Equipe>> ListarEquipeAsync();
    Task<IEnumerable<Equipe>> ListarComMembrosAsync();
}
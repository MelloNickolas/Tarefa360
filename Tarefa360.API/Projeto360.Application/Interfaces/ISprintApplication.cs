using Projeto360.Domain.Entities;
#nullable enable

namespace Projeto360.Application.Interfaces;

public interface ISprintApplication
{
    Task<int> CriarAsync(Sprint sprint);
    Task<bool> AtualizarAsync(Sprint sprint);
    Task<Sprint?> ObterPorIdAsync(int id);
    Task<IEnumerable<Sprint>> ListarAsync();
    Task<bool> DeletarAsync(int id);

    /* 
    Método para buscar sprints por título.
    Pode retornar vários resultados semelhantes.
    */
    Task<IEnumerable<Sprint>> ObterPorTituloAsync(string titulo);

    /* 
    Lista sprints filtrando por projeto.
    */
    Task<IEnumerable<Sprint>> ListarPorProjetoAsync(int projetoId);
}
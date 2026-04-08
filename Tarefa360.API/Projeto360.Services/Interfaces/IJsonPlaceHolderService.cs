using Projeto360.Domain.Entities;

namespace Projeto360.Services.Interfaces
{
    public interface IJsonPlaceHolderService
    {
        Task<List<Tarefa>> ListarTarefas();
    }
}
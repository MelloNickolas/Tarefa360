using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Interfaces;
public interface IHistoriaRepository
{
  Task<int> SalvarAsync(Historia historia);
  Task AtualizarAsync(Historia historia);
  Task<Historia> ObterPorIdAsync(int id);
  Task<IEnumerable<Historia>> ListarAsync();
  Task DeletarAsync(Historia historia);

  /* 
  Método para buscar noss historia por nome! 
  Ele se torna um IEnumerable pois quando digitar exemplo (Hist), pode conter mais de 1 historia que tenho esse nome!

  Colocamos um método para buscar por projeto tambem na hora de filtrar tudo!
  */
  Task<IEnumerable<Historia>> ObterPorNomeAsync(string nome);
  Task<IEnumerable<Historia>> ListarPorProjetoAsync(int projetoId);
}
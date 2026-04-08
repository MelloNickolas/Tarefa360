using Microsoft.EntityFrameworkCore;
using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;

namespace Projeto360.Repository;

public class HistoriaRepository : BaseRepository, IHistoriaRepository
{
  public HistoriaRepository(Projeto360Context context) : base(context) { }


  public async Task<Historia> ObterPorIdAsync(int id)
  {
    /* Esse INCLUDE serve para carregarmos o projeto junto para assim podermos mostrar o nome dele */
    return await _context.Historias.Include(historia => historia.Projeto).AsNoTracking().Where(historia => historia.ID == id).FirstOrDefaultAsync();
  }
  public async Task AtualizarAsync(Historia historia)
  {
    _context.Historias.Update(historia);
    await _context.SaveChangesAsync();
  }

  public async Task DeletarAsync(Historia historia)
  {
    _context.Historias.Remove(historia);
    await _context.SaveChangesAsync();
  }

  public async Task<IEnumerable<Historia>> ListarAsync()
  {
    return await _context.Historias.Include(historia => historia.Projeto).ToListAsync();
  }

  public async Task<int> SalvarAsync(Historia historia)
  {
    await _context.Historias.AddAsync(historia);
    await _context.SaveChangesAsync();

    return historia.ID;
  }



  public async Task<IEnumerable<Historia>> ObterPorNomeAsync(string nome)
  {
    /*Possui o .Contains pq as vezes a pessoa nao procura pelo nome exato entao quero trazer todos que tem aqls letras na barra de busca*/
    return await _context.Historias.Include(historia => historia.Projeto).Where(historia => historia.Nome.ToLower() .Contains(nome.ToLower())).ToListAsync();
  }

  public async Task<IEnumerable<Historia>> ListarPorProjetoAsync(int projetoId)
  {
    return await _context.Historias
    .Include(historia => historia.Projeto)
    .Where(h => h.ProjetoID == projetoId)
    .ToListAsync();
  }
}
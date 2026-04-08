using Microsoft.EntityFrameworkCore;
using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;

namespace Projeto360.Repository;

public class EquipeRepository : BaseRepository, IEquipeRepository
{
  public EquipeRepository(Projeto360Context context) : base(context) { }

  public async Task AtualizarEquipeAsync(Equipe equipe)
  {
    _context.Equipes.Update(equipe);
    await _context.SaveChangesAsync();
  }

  public async Task<int> CriarEquipeAsync(Equipe equipe)
  {
    _context.Equipes.Add(equipe);
    await _context.SaveChangesAsync();

    return equipe.ID;
  }

  public async Task DeletarEquipeAsync(Equipe equipe)
  {
    _context.Equipes.Remove(equipe);
    await _context.SaveChangesAsync();
  }

  public async Task<IEnumerable<Equipe>> ListarEquipesAsync()
  {
    return await _context.Equipes.ToListAsync();
  }

  public async Task<IEnumerable<Equipe>> ListarMembrosPorEquipeAsync()
  {
    return await _context.Equipes
    .Include(e => e.UsuariosEquipe)
        .ThenInclude(ue => ue.Usuario)
    .ToListAsync();
  }

  public async Task<Equipe> ObterEquipePorIdAsync(int id)
  {
    return await _context.Equipes.FirstOrDefaultAsync(equipe => equipe.ID == id);
  }

  public async Task<IEnumerable<Equipe>> ObterEquipePorNomeAsync(string nome)
  {
    return await _context.Equipes.Where(equipe => equipe.Nome.ToLower().Contains(nome.ToLower())).ToListAsync();
  }

  public async Task<IEnumerable<Equipe>> ObterEquipePorNomeExatoAsync(string nome)
  {
    return await _context.Equipes
        .Where(equipe => equipe.Nome.ToLower() == nome.ToLower())
        .ToListAsync();
  }
}
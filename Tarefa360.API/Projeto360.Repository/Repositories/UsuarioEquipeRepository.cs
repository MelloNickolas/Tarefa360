using Microsoft.EntityFrameworkCore;
using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;

namespace Projeto360.Repository;

public class UsuarioEquipeRepository : BaseRepository, IUsuarioEquipeRepository
{
  public UsuarioEquipeRepository(Projeto360Context context) : base(context)
  {
  }

  public async Task<int> CriarAsync(UsuarioEquipe usuarioEquipe)
  {
    _context.Add(usuarioEquipe);
    await _context.SaveChangesAsync();

    return usuarioEquipe.ID;
  }

  public async Task DeletarAsync(UsuarioEquipe usuarioEquipe)
  {
    _context.UsuariosEquipes.Remove(usuarioEquipe);
    await _context.SaveChangesAsync();
  }

  public async Task<IEnumerable<UsuarioEquipe>> ListarPorEquipesAsync(int equipeID)
  {
    return await _context.UsuariosEquipes
        .Include(ue => ue.Usuario)
        .Include(ue => ue.Equipe)
        .Where(ue => ue.EquipeId == equipeID)
        .ToListAsync();
  }

  public async Task<IEnumerable<UsuarioEquipe>> ListarPorUsuariosAsync(int usuarioID)
  {
    return await _context.UsuariosEquipes
        .Include(ue => ue.Usuario)
        .Include(ue => ue.Equipe)
        .Where(ue => ue.UsuarioId == usuarioID)
        .ToListAsync();
  }
  public async Task<UsuarioEquipe> ObterPorIdAsync(int id)
  {
    return await _context.UsuariosEquipes
        .Include(ue => ue.Usuario)
        .Include(ue => ue.Equipe)
        .FirstOrDefaultAsync(ue => ue.ID == id);
  }

  public async Task<UsuarioEquipe> AtualizarPapelPorIdAsync(UsuarioEquipe usuarioEquipe)
  {
    _context.UsuariosEquipes.Update(usuarioEquipe);
    await _context.SaveChangesAsync();
    return usuarioEquipe;
  }
}
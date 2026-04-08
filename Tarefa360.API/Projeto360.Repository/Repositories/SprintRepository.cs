using Microsoft.EntityFrameworkCore;
using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;

namespace Projeto360.Repository;

public class SprintRepository : BaseRepository, ISprintRepository
{
    public SprintRepository(Projeto360Context context) : base(context) { }
    // busca por ID 
    public async Task<Sprint> ObterPorIdAsync(int id)
    {
        return await _context.Sprints
            .Include(s => s.Projeto)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ID == id);
    }
     // atualiza 
    public async Task AtualizarAsync(Sprint sprint)
    {
        _context.Sprints.Update(sprint);
        await _context.SaveChangesAsync();
    }
    // delete 
    public async Task DeletarAsync(Sprint sprint)
    {
        _context.Sprints.Remove(sprint);
        await _context.SaveChangesAsync();
    }
    // lista todos 
    public async Task<IEnumerable<Sprint>> ListarAsync()
    {
        return await _context.Sprints
            .Include(s => s.Projeto)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> SalvarAsync(Sprint sprint)
    {
        await _context.Sprints.AddAsync(sprint);
        await _context.SaveChangesAsync();

        return sprint.ID;
    }
    // Busca por titulo 
    public async Task<IEnumerable<Sprint>> ObterPorTituloAsync(string titulo)
    {
        return await _context.Sprints
            .Include(s => s.Projeto)
            .AsNoTracking()
            .Where(s => s.Titulo.Contains(titulo))
            .ToListAsync();
    }
    // lista por projetos 
    public async Task<IEnumerable<Sprint>> ListarPorProjetoAsync(int projetoId)
    {
        return await _context.Sprints
            .Include(s => s.Projeto)
            .AsNoTracking()
            .Where(s => s.ProjetoID == projetoId)
            .ToListAsync();
    }
}
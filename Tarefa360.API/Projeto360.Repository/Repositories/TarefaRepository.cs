using Microsoft.EntityFrameworkCore;
using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;

namespace Projeto360.Repository;

public class TarefaRepository : BaseRepository, ITarefaRepository
{
    public TarefaRepository(Projeto360Context context) : base(context)
    {
    }

    public async Task<int> CriarAsync(Tarefa tarefa)
    {
        _context.Tarefas.Add(tarefa);
        await _context.SaveChangesAsync();

        return tarefa.ID;
    }

    public async Task AtualizarAsync(Tarefa tarefa)
    {
        _context.Tarefas.Update(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(Tarefa tarefa)
    {
        _context.Tarefas.Remove(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Tarefa>> ListarAsync(bool concluida)
    {
        return await _context.Tarefas
            .Where(t => t.Concluido == concluida)
            .Include(u => u.Usuario)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> ListarTodasAsync()
    {
        return await _context.Tarefas
        .Include(t => t.Usuario)
        .ToListAsync();
    }

    public async Task<Tarefa> ObterPorIdAsync(int id)
    {
        return await _context.Tarefas
            .Include(u => u.Usuario)
            .FirstOrDefaultAsync(u => u.ID == id);
    }
}
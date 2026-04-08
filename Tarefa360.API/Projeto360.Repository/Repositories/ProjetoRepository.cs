using Microsoft.EntityFrameworkCore;
using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;

namespace Projeto360.Repository;

public class ProjetoRepository : BaseRepository, IProjetoRepository
{
    public ProjetoRepository(Projeto360Context context) : base(context)
    {
    }
    public async Task<int> CriarProjetoAsync(Projeto projeto)
    {
        _context.Projetos.Add(projeto);
        await _context.SaveChangesAsync();

        return projeto.ID;
    }

    public async Task AtualizarProjetoAsync(Projeto projeto)
    {
        _context.Projetos.Update(projeto);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarProjetoAsync(Projeto projeto)
    {
        _context.Projetos.Remove(projeto);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Projeto>> ListarProjetoAsync(bool ativo)
    {
        return await _context.Projetos
                .Where(u => u.Ativo == ativo)
                .ToListAsync();
    }


    public async Task<Projeto> ObterProjetoPorIdAsync(int id)
    {
        return await _context.Projetos
                .FirstOrDefaultAsync(u => u.ID == id);
    }

     public async Task<Projeto> ObterProjetoPorNomeAsync(string nome)
    {
        return await _context.Projetos
                .Where(u => u.Ativo)
                .FirstOrDefaultAsync(u => u.Nome == nome);
    }
}
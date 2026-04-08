using Microsoft.EntityFrameworkCore;
using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;

namespace Projeto360.Repository;
public class TwoFactorRepository : BaseRepository, ITwoFactorRepository
{
    public TwoFactorRepository(Projeto360Context context) : base(context)
    {
    }
    // Persiste o novo token no banco
    public async Task CriarAsync(TwoFactorToken token)
    {
        _context.TwoFactorTokens.Add(token);
        await _context.SaveChangesAsync();
    }

    // Busca token que: pertence ao usuário + código correto + não expirou + não foi
    public async Task<TwoFactorToken> ObterTokenValidoAsync(int usuarioId, string codigo)
    {
        return await _context.TwoFactorTokens
        .Where(t => t.UsuarioID == usuarioId
        && t.Codigo == codigo
        && !t.Utilizado
        && t.Expiracao > DateTime.UtcNow)
        .FirstOrDefaultAsync();
    }

    // Marca o token como utilizado após o usuário confirmar o código
    public async Task MarcarComoUtilizadoAsync(TwoFactorToken token)
    {
        token.Utilizado = true;

        _context.TwoFactorTokens.Update(token);
        await _context.SaveChangesAsync();
    }

    // Quando o usuário pede um novo código, invalida os anteriores para segurança
    public async Task InvalidarTokensAnterioresAsync(int usuarioId)
    {
        var tokensPendentes = await _context.TwoFactorTokens
        .Where(t => t.UsuarioID == usuarioId && !t.Utilizado)
        .ToListAsync();
        
        foreach (var t in tokensPendentes)
        {
            t.Utilizado = true;
        }
            
        await _context.SaveChangesAsync();
    }
}
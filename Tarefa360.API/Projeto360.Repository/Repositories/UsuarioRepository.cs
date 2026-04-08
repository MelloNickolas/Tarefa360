using Microsoft.EntityFrameworkCore;
using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;
using BCrypt.Net;

namespace Projeto360.Repository;

public class UsuarioRepository : BaseRepository, IUsuarioRepository
{
    public UsuarioRepository(Projeto360Context context) : base(context)
    {
    }
    public async Task<int> CriarAsync(Usuario usuario)
    {
        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return usuario.ID;
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Usuario>> ListarAsync(bool ativo)
    {
        return await _context.Usuarios
                .Where(u => u.Ativo == ativo)
                .ToListAsync();
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        return await _context.Usuarios
                .Where(u => u.Ativo)
                .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario> ObterPorIdAsync(int id)
    {
        return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.ID == id);
    }

    public async Task<Usuario> ObterPorEmailESenhaAsync(string email, string senha)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email && u.Ativo);
        
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.Senha))
        {
            return null;
        }
        
        return usuario;
    }

    // Busca o usuário que possui exatamente esse RefreshToken
    public async Task<Usuario> ObterPorRefreshTokenAsync(string refreshToken)
    {
        return await _context.Usuarios
        .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.Ativo);
    }
}
using Microsoft.EntityFrameworkCore;
using Projeto360.Dominio.Entidades;

namespace Projeto360.Repositories
{
  public class UsuarioRepository : BaseRepository, IUsuarioRepository
  {
    public UsuarioRepository(Projeto360Context context) : base(context)
    {
    }
    public void Atualizar(Usuario usuario)
    {
      _context.Usuarios.Update(usuario);
      _context.SaveChanges();
    }

    public IEnumerable<Usuario> Listar(bool ativo)
    {
      return _context.Usuarios.Where(u => u.Ativo == true);
    }

    public Usuario Obter(int IdUsuario)
    {
      return _context.Usuarios
      .Where(u => u.IDUsuario == IdUsuario)
      .FirstOrDefault();
    }

    public Usuario ObterPorEmail(string Email)
    {
      return _context.Usuarios.Where(u => u.Email == Email).Where(u => u.Ativo == true).FirstOrDefault();
    }

    public int Salvar(Usuario usuario)
    {
      _context.Usuarios.Add(usuario);
      _context.SaveChanges();
      return usuario.IDUsuario;
    }






    public async Task<int> SalvarAsync(Usuario usuario)
    {
      await _context.Usuarios.AddAsync(usuario);
      await _context.SaveChangesAsync();
      return usuario.IDUsuario;
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
      _context.Usuarios.Update(usuario);
      await _context.SaveChangesAsync();
    }

    public async Task<Usuario> ObterAsync(int IdUsuario)
    {
      return await _context.Usuarios
        .Where(u => u.IDUsuario == IdUsuario)
        .FirstOrDefaultAsync();
    }

    public async Task<Usuario> ObterPorEmailAsync(string Email)
    {
      return await _context.Usuarios
        .Where(u => u.Email == Email)
        .Where(u => u.Ativo == true)
        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Usuario>> ListarAsync(bool ativo)
    {
      return await _context.Usuarios
        .Where(u => u.Ativo == true)
        .ToListAsync();
    }
  }
}
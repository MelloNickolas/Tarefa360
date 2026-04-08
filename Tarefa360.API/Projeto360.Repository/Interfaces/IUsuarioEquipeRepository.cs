using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Interfaces;

public interface IUsuarioEquipeRepository
{
  Task<int> CriarAsync(UsuarioEquipe usuarioEquipe);
  Task<UsuarioEquipe> ObterPorIdAsync(int id);
  Task<UsuarioEquipe> AtualizarPapelPorIdAsync(UsuarioEquipe usuarioEquipe);
  Task<IEnumerable<UsuarioEquipe>> ListarPorUsuariosAsync(int usuarioID);
  Task<IEnumerable<UsuarioEquipe>> ListarPorEquipesAsync(int equipeID);
  Task DeletarAsync(UsuarioEquipe usuarioEquipe);
}
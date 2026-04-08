using Projeto360.Domain.Entities;
using Projeto360.Domain.Enums;

namespace Projeto360.Application.Interfaces;

public interface IUsuarioEquipeApplication
{
  Task<int> CriarAsync(UsuarioEquipe usuarioEquipe);
  Task<UsuarioEquipe> ObterPorIdAsync(int id);
  Task<IEnumerable<UsuarioEquipe>> ListarPorUsuariosAsync(int usuarioID);
  Task<IEnumerable<UsuarioEquipe>> ListarPorEquipesAsync(int equipeID);
  Task<UsuarioEquipe> AtualizarPapelPorIdAsync(UsuarioEquipe usuarioEquipe);
  Task DeletarAsync(UsuarioEquipe usuarioEquipe);
  Dictionary<PapeisEquipe, string> ListarPapeisEquipeAsync();
}
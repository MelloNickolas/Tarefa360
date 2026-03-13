using Projeto360.Dominio.Entidades;

public interface IUsuarioRepository
{
  //Métodos comuns para realizar, estou usando TASK para nao travar meu codigo que nem recomendaram na revisão!
  public int Salvar(Usuario usuario);
  public void Atualizar(Usuario usuario);
  public Usuario Obter(int IdUsuario);
  public Usuario ObterPorEmail(string Email);
  public IEnumerable<Usuario> Listar(bool ativo);


  //Metodos Async
  Task<int> SalvarAsync(Usuario usuario);
  Task AtualizarAsync(Usuario usuario);
  Task<Usuario> ObterAsync(int IdUsuario);
  Task<Usuario> ObterPorEmailAsync(string Email);
  Task<IEnumerable<Usuario>> ListarAsync(bool ativo);
}
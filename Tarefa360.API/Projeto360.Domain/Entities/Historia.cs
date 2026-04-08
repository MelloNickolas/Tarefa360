
namespace Projeto360.Domain.Entities;

public class Historia
{
  public int ID { get; set; }
  public string Nome { get; set; }
  public string Descricao { get; set; }

  // Campo para relacionamento, Vamos puxar o ID para relacionar o projeto
  public int ProjetoID { get; set; }
  public Projeto Projeto { get; set; }
  /* Esse campo serve para podermos navegar nas propriedades do projeto relacionado */

  // Ver tarefas relaciondas a historia
  public ICollection<Tarefa> Tarefas { get; set; }

}
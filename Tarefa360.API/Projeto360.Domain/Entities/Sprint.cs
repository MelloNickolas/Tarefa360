namespace Projeto360.Domain.Entities;

public class Sprint
{
  public int ID { get; set; }
  public string Titulo { get; set; }
  public string Descricao { get; set; }
  public DateTime DataInicio { get; set; }
  public DateTime DataFim { get; set; }

  // Campo para relacionamento, Vamos puxar o ID para relacionar o projeto
  public int ProjetoID { get; set; }
  public Projeto Projeto { get; set; }
  /* Esse campo serve para podermos navegar nas propriedades do projeto relacionado */


  public ICollection<Tarefa> Tarefas { get; set; }
}
namespace Projeto360.Api.Models.Request;

public class SprintAtualizar
{
  public string Titulo { get; set; }
  public string Descricao { get; set; }
  public DateTime DataInicio { get; set; }
  public DateTime DataFim { get; set; }
  public int ProjetoId { get; set; } 
}
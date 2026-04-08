namespace Projeto360.Api.Models.Response;

public class SprintResponse
{
  public int ID { get; set; }
  public string Titulo { get; set; }
  public string Descricao { get; set; }
  public int ProjetoId { get; set; }
  public string NomeProjeto { get; set; }
}
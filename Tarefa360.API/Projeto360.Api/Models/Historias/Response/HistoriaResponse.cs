namespace Projeto360.Api.Models.Response;

public class HistoriaResponse
{
  public int ID { get; set; }
  public string Nome { get; set; }
  public string Descricao { get; set; }
  public int ProjetoId { get; set; }
  public string NomeProjeto { get; set; }
}
using Projeto360.Domain.Enums;

namespace Projeto360.Api.Models.Response;

public class TarefaResponse
{
    public int ID { get; set; }
    public DateTime DataCriacao { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public decimal? Estimativa { get; set; }
    public int TipoTarefaID { get; set; }
    public bool Concluido { get; set; }
    public DateTime? DataConclusao { get; set; }
    public int ProjetoID { get; set; }
    public int HistoriaID { get; set; }
    public UsuarioResponse Usuario { get; set; }
    public int SprintID { get; set; }
}
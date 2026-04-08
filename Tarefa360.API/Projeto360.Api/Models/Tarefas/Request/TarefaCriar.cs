using Projeto360.Domain.Enums;

namespace Projeto360.Api.Models.Request;

public class TarefaRequest
{
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public decimal? Estimativa { get; set; }
    public int TipoTarefaID { get; set; }
    public int ProjetoID { get; set; }
    public int HistoriaID { get; set; }
    public int UsuarioID { get; set; }
    public int SprintID { get; set; }
}
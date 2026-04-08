using Projeto360.Domain.Enums;

namespace Projeto360.Api.Models.Response;

public class TarefasPorTipoResponse
{
    public string TipoTarefa { get; set; }
    public int Quantidade { get; set; }
}

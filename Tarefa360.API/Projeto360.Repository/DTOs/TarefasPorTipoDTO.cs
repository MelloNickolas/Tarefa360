using Projeto360.Domain.Enums;

namespace Projeto360.Repository.DTOs;

public class TarefasPorTipoDTO
{
    public TiposTarefa TipoTarefa { get; set; }
    public int Qtd_tarefas { get; set; }
}
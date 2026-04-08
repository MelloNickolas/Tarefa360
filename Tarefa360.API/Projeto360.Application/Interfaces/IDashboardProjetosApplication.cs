using Projeto360.Domain.Entities;
using Projeto360.Domain.Enums;
using Projeto360.Repository.DTOs;

namespace Projeto360.Application.Interfaces;
public interface IDashboardProjetosApplication
{
    Task<int> QtdTotalHistoriasAsync();
    Task<int> QtdHistoriasPorProjetoAsync(int ProjetoID);
    Task<IEnumerable<HistoriasPorProjetoDTO>> QtdHistoriasPorProjetoAgrupadoAsync();
    Task<int> QtdHistoriasPorConclusaoAsync(bool concluido);
    Task<IEnumerable<TarefasPorTipoDTO>> QtdTotalTarefasAgrupadoPorTipoAsync();
    Task<IEnumerable<TarefasPorTipoDTO>> QtdTarefasAgrupadoPorTipoPorConclusaoAsync(bool concluido);
    Task<int> QtdHorasPorConclusaoAsync(bool concluido);
    Task<int> QtdTotalTarefasPorConclusaoAsync(bool Concluido);
    Task<int> QtdTotalTarefasPorTipoPorConclusaoAsync(TiposTarefa tipoTarefa, bool Concluido);
    Task<IEnumerable<Tarefa>> ListarTarefasConcluidasDataAtualAsync();
}
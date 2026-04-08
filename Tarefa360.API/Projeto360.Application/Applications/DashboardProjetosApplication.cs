using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;
using Projeto360.Application.Interfaces;
using Projeto360.Repository.DTOs;
using Projeto360.Domain.Enums;

namespace Projeto360.Application;

public class DashboardProjetosApplication : IDashboardProjetosApplication
{
    private readonly IDashboardProjetosRepository _dashboardProjetosRepository;
    private readonly IProjetoApplication _projetoApplication;
    
    public DashboardProjetosApplication(IDashboardProjetosRepository dashboardProjetosRepository, IProjetoApplication projetoApplication)
    {
        _dashboardProjetosRepository = dashboardProjetosRepository;
        _projetoApplication = projetoApplication;
    }

    public async Task<int> QtdTotalHistoriasAsync() 
        => await _dashboardProjetosRepository.QtdTotalHistoriasAsync();
    
    public async Task<int> QtdHistoriasPorProjetoAsync(int ProjetoID)
    {
        Projeto projetoExistente = await ObterProjetoPorId(ProjetoID);
        return await _dashboardProjetosRepository.QtdHistoriasPorProjetoAsync(projetoExistente.ID);
    }

    public async Task<IEnumerable<HistoriasPorProjetoDTO>> QtdHistoriasPorProjetoAgrupadoAsync() 
        => await _dashboardProjetosRepository.QtdHistoriasPorProjetoAgrupadoAsync();

    public async Task<int> QtdHistoriasPorConclusaoAsync(bool concluido)
        => await _dashboardProjetosRepository.QtdHistoriasPorConclusaoAsync(concluido);

    public async Task<IEnumerable<TarefasPorTipoDTO>> QtdTotalTarefasAgrupadoPorTipoAsync() 
        => await _dashboardProjetosRepository.QtdTotalTarefasAgrupadoPorTipoAsync();
        
    public async Task<IEnumerable<TarefasPorTipoDTO>> QtdTarefasAgrupadoPorTipoPorConclusaoAsync(bool concluido) 
        => await _dashboardProjetosRepository.QtdTarefasAgrupadoPorTipoPorConclusaoAsync(concluido);

    public async Task<int> QtdHorasPorConclusaoAsync(bool concluido) 
        => await _dashboardProjetosRepository.QtdHorasPorConclusaoAsync(concluido);

    public async Task<int> QtdTotalTarefasPorConclusaoAsync(bool concluido) 
        => await _dashboardProjetosRepository.QtdTotalTarefasPorConclusaoAsync(concluido);

    public async Task<int> QtdTotalTarefasPorTipoPorConclusaoAsync(TiposTarefa tipoTarefa, bool concluido)
    {
        // Valida se o tipo de tarefa foi preenchido e é um tipo valido do enum
        if (!Enum.IsDefined(typeof(TiposTarefa), tipoTarefa))
            throw new Exception("Tipo de tarefa inválido.");
        
        return await _dashboardProjetosRepository.QtdTotalTarefasPorTipoPorConclusaoAsync(tipoTarefa, concluido);
    }

    public async Task<IEnumerable<Tarefa>> ListarTarefasConcluidasDataAtualAsync() 
        => await _dashboardProjetosRepository.ListarTarefasConcluidasDataAtualAsync();
    
    #region Úteis
    // No application já tem a valiação se existe com esse ID, 
    // então aqui só precisa fazer o get
    // Se não existir vai estourar Exception
    private async Task<Projeto> ObterProjetoPorId(int projetoId) 
        => await _projetoApplication.ObterProjetoPorIdAsync(projetoId);

    #endregion

}
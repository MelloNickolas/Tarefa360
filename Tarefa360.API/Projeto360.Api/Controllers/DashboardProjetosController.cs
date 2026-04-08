using Microsoft.AspNetCore.Mvc;
using Projeto360.Api.Models.Request;
using Projeto360.Api.Models.Response;
using Projeto360.Domain.Enums;
using Projeto360.Domain.Entities;
using Projeto360.Application.Interfaces;

namespace Projeto360.Api;

[ApiController]
[Route("[controller]")]
public class DashboardProjetosController : ControllerBase
{
    private readonly IDashboardProjetosApplication _dashboardProjetosApplication;

    public DashboardProjetosController(IDashboardProjetosApplication dashboardProjetosApplication)
    {
        _dashboardProjetosApplication = dashboardProjetosApplication;
    }

    [HttpGet]
    [Route("QtdTotalHistorias")]
    public async Task<ActionResult> QtdTotalHistoriasAsync()
    {
        try
        {
            var response = await _dashboardProjetosApplication.QtdTotalHistoriasAsync();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("QtdHistoriasPorProjeto")]
    public async Task<ActionResult> QtdHistoriasPorProjetoAsync(int ProjetoID)
    {
        try
        {
            var response = await _dashboardProjetosApplication.QtdHistoriasPorProjetoAsync(ProjetoID);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("QtdHistoriasPorProjetoAgrupado")]
    public async Task<ActionResult> QtdHistoriasPorProjetoAgrupadoAsync()
    {
        try
        {
            var response = await _dashboardProjetosApplication.QtdHistoriasPorProjetoAgrupadoAsync();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("QtdHistoriasPorConclusao")]
    public async Task<ActionResult> QtdHistoriasPorConclusaoAsync([FromQuery] bool concluido)
    {
        try
        {
            var response = await _dashboardProjetosApplication.QtdHistoriasPorConclusaoAsync(concluido);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("QtdTotalTarefasAgrupadoPorTipo")]
    public async Task<ActionResult> QtdTotalTarefasAgrupadoPorTipoAsync()
    {
        try
        {
            var response = await _dashboardProjetosApplication.QtdTotalTarefasAgrupadoPorTipoAsync();

            var responseFormatada = response.Select(r => new TarefasPorTipoResponse
            {
                TipoTarefa = ((TiposTarefa)r.TipoTarefa).ToString(),
                Quantidade = r.Qtd_tarefas
            }).ToList();

            return Ok(responseFormatada);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("QtdTotalTarefasPorTipoPorConclusao")]
    public async Task<ActionResult> QtdTotalTarefasPorTipoPorConclusaoAsync([FromQuery] bool concluido, [FromQuery] TiposTarefa tipoTarefa)
    {
        try
        {
            var response = await _dashboardProjetosApplication.QtdTotalTarefasPorTipoPorConclusaoAsync(tipoTarefa, concluido);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("QtdTarefasAgrupadoPorTipoPorConclusao")]
    public async Task<ActionResult> QtdTarefasAgrupadoPorTipoPorConclusaoAsync([FromQuery] bool concluido)
    {
        try
        {
            var response = await _dashboardProjetosApplication.QtdTarefasAgrupadoPorTipoPorConclusaoAsync(concluido);
            
            var responseFormatada = response.Select(r => new TarefasPorTipoResponse
            {
                TipoTarefa = ((TiposTarefa)r.TipoTarefa).ToString(),
                Quantidade = r.Qtd_tarefas
            }).ToList();

            return Ok(responseFormatada);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("QtdTotalTarefasPorConclusao")]
    public async Task<ActionResult> QtdTotalTarefasPorConclusaoAsync([FromQuery] bool concluido)
    {
        try
        {
            var response = await _dashboardProjetosApplication.QtdTotalTarefasPorConclusaoAsync(concluido);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet]
    [Route("QtdHorasPorConclusao")]
    public async Task<ActionResult> QtdHorasPorConclusaoAsync([FromQuery] bool concluido)
    {
        try
        {
            var response = await _dashboardProjetosApplication.QtdHorasPorConclusaoAsync(concluido);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("ListarTarefasConcluidasDataAtual")]
    public async Task<ActionResult> ListarTarefasConcluidasDataAtual()
    {
        try
        {
            var tarefas = await _dashboardProjetosApplication.ListarTarefasConcluidasDataAtualAsync();
            var response = tarefas.Select(t => new TarefaResponse()
            {
                ID = t.ID,
                DataCriacao = t.DataCriacao,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Estimativa = t.Estimativa,
                TipoTarefaID = (int)t.TipoTarefa,
                Concluido = t.Concluido,
                DataConclusao = t.DataConclusao,
                ProjetoID = t.ProjetoID,
                HistoriaID = t.HistoriaID,
                Usuario = new UsuarioResponse
                {
                    Id = t.UsuarioID,
                    Nome = t.Usuario.Nome,
                    Email = t.Usuario.Email,
                    TipoUsuarioId = (int)t.Usuario.TipoUsuario
                }
                // SprintID = t.SprintID
            });

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
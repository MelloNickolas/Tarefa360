using Microsoft.AspNetCore.Mvc;
using Projeto360.Api.Models.Request;
using Projeto360.Api.Models.Response;
using Projeto360.Domain.Enums;
using Projeto360.Domain.Entities;
using Projeto360.Application.Interfaces;

namespace Projeto360.Api;

[ApiController]
[Route("[controller]")]
public class TarefaController : ControllerBase
{
    private readonly ITarefaApplication _tarefaApplication;

    public TarefaController(ITarefaApplication tarefaApplication)
    {
        _tarefaApplication = tarefaApplication;
    }

    [HttpGet]
    [Route("ListarTodas")]
    public async Task<ActionResult> ListarTodas()
    {
        try
        {
            var tarefasDominio = await _tarefaApplication.ListarTodasAsync();

            var tarefas = tarefasDominio.Select(t => new TarefaResponse()
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
                    Id = t.Usuario.ID,
                    Nome = t.Usuario.Nome,
                    Email = t.Usuario.Email,
                    TipoUsuarioId = (int)t.Usuario.TipoUsuario
                },
                SprintID = t.SprintID
            });

            return Ok(tarefas);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("ListarPorStatus")]
    public async Task<ActionResult> ListarPorStatus([FromQuery] bool concluida)
    {
        try
        {
            var tarefasDominio = await _tarefaApplication.ListarAsync(concluida);

            var tarefas = tarefasDominio.Select(t => new TarefaResponse()
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
                    Id = t.Usuario.ID,
                    Nome = t.Usuario.Nome,
                    Email = t.Usuario.Email,
                    TipoUsuarioId = (int)t.Usuario.TipoUsuario
                },
                SprintID = t.SprintID
            });

            return Ok(tarefas);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("ListarTiposTarefa")]
    public async Task<ActionResult> ListarTiposTarefa()
    {
        try
        {
            var tiposTarefa = await _tarefaApplication.ListarTiposTarefaAsync();
            var TiposTarefaResponse = tiposTarefa.Select(t => new TiposTarefaResponse()
            {
                Id = (int)t.Key,
                Nome = t.Value
            }).ToList();

            return Ok(TiposTarefaResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("ObterPorId/{tarefaId}")]
    public async Task<ActionResult> ObterPorId([FromRoute] int tarefaId)
    {
        try
        {
            var tarefaDominio = await _tarefaApplication.ObterPorIdAsync(tarefaId);

            var tarefaResponse = new TarefaResponse()
            {
                ID = tarefaDominio.ID,
                DataCriacao = tarefaDominio.DataCriacao,
                Titulo = tarefaDominio.Titulo,
                Descricao = tarefaDominio.Descricao,
                Estimativa = tarefaDominio.Estimativa,
                TipoTarefaID = (int)tarefaDominio.TipoTarefa,
                Concluido = tarefaDominio.Concluido,
                DataConclusao = tarefaDominio.DataConclusao,
                ProjetoID = tarefaDominio.ProjetoID,
                HistoriaID = tarefaDominio.HistoriaID,
                Usuario = new UsuarioResponse
                {
                    Id = tarefaDominio.Usuario.ID,
                    Nome = tarefaDominio.Usuario.Nome,
                    Email = tarefaDominio.Usuario.Email,
                    TipoUsuarioId = (int)tarefaDominio.Usuario.TipoUsuario
                },
                SprintID = tarefaDominio.SprintID
            };

            return Ok(tarefaResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("Criar")]
    public async Task<ActionResult> Criar([FromBody] TarefaRequest tarefaCriar)
    {
        try
        {
            var tarefaDominio = new Tarefa()
            {
                Titulo = tarefaCriar.Titulo,
                Descricao = tarefaCriar.Descricao,
                Estimativa = tarefaCriar.Estimativa,
                TipoTarefa = (TiposTarefa)tarefaCriar.TipoTarefaID,
                ProjetoID = tarefaCriar.ProjetoID,
                HistoriaID = tarefaCriar.HistoriaID,
                UsuarioID = tarefaCriar.UsuarioID,
                SprintID = tarefaCriar.SprintID
            };

            var tarefaId = await _tarefaApplication.CriarAsync(tarefaDominio);

            return CreatedAtAction(nameof(ObterPorId), new { tarefaId }, tarefaId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Route("Atualizar/{tarefaId}")]
    public async Task<ActionResult> Atualizar([FromRoute] int tarefaId, [FromBody] TarefaRequest tarefaAtualizar)
    {
        try
        {
            var tarefaDominio = new Tarefa()
            {
                ID = tarefaId,
                Titulo = tarefaAtualizar.Titulo,
                Descricao = tarefaAtualizar.Descricao,
                Estimativa = tarefaAtualizar.Estimativa,
                TipoTarefa = (TiposTarefa)tarefaAtualizar.TipoTarefaID,
                ProjetoID = tarefaAtualizar.ProjetoID,
                HistoriaID = tarefaAtualizar.HistoriaID,
                UsuarioID = tarefaAtualizar.UsuarioID,
                SprintID = tarefaAtualizar.SprintID
            };

            await _tarefaApplication.AtualizarAsync(tarefaDominio);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut]
    [Route("Concluir/{tarefaId}")] 
    public async Task<ActionResult> Concluir([FromRoute] int tarefaId)
    {
        try
        {
            await _tarefaApplication.ConcluirTarefaAsync(tarefaId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Route("Retomar/{tarefaId}")] 
    public async Task<ActionResult> Retomar([FromRoute] int tarefaId)
    {
        try
        {
            await _tarefaApplication.RetomarTarefaAsync(tarefaId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("Deletar/{tarefaId}")]
    public async Task<ActionResult> Deletar([FromRoute] int tarefaId)
    {
        try
        {
            await _tarefaApplication.DeletarAsync(tarefaId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
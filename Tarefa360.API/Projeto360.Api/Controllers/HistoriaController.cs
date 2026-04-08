using Microsoft.AspNetCore.Mvc;
using Projeto360.Api.Models.Request;
using Projeto360.Api.Models.Response;
using Projeto360.Application.Interfaces;
using Projeto360.Domain.Entities;

namespace Projeto360.Api;

[ApiController]
[Route("[controller]")]
public class HistoriaController : ControllerBase
{
  private readonly IHistoriaApplication _historiaApplication;
  public HistoriaController(IHistoriaApplication historiaApplication)
  {
    _historiaApplication = historiaApplication;
  }


  [HttpPost]
  [Route("CriarHistoria")]
  public async Task<ActionResult> CriarHistoria([FromBody] HistoriaCriar historiaCriar)
  {
    try
    {
      var historiaDominio = new Historia()
      {
        Nome = historiaCriar.Nome,
        Descricao = historiaCriar.Descricao,
        ProjetoID = historiaCriar.ProjetoId
      };

      var IdHistoria = await _historiaApplication.SalvarAsync(historiaDominio);

      return Ok(IdHistoria);
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }

  [HttpGet]
  [Route("ObterHistoriaPorId/{historiaId}")]
  public async Task<ActionResult> ObterHistoriaPorId([FromRoute] int historiaId)
  {
    try
    {
      var historiaDominio = await _historiaApplication.ObterPorIdAsync(historiaId);

      var historiaResponse = new HistoriaResponse()
      {
        ID = historiaDominio.ID,
        Nome = historiaDominio.Nome,
        Descricao = historiaDominio.Descricao,
        ProjetoId = historiaDominio.ProjetoID,
        NomeProjeto = historiaDominio.Projeto.Nome
      };
      return Ok(historiaResponse);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }


  [HttpGet]
  [Route("ObterHistoriaPorNome")]
  public async Task<ActionResult> ObterHistoriaPorNome([FromQuery] string nome)
  {
    try
    {
      var historiaDominio = await _historiaApplication.ObterPorNomeAsync(nome);

      var historias = historiaDominio.Select(h => new HistoriaResponse()
      {
        ID = h.ID,
        Nome = h.Nome,
        Descricao = h.Descricao,
        ProjetoId = h.ProjetoID,
        NomeProjeto = h.Projeto.Nome
      });
      return Ok(historias);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpGet]
  [Route("ListarHistorias")]
  public async Task<ActionResult> ListarHistorias()
  {
    try
    {
      var historiaDominio = await _historiaApplication.ListarAsync();

      var historias = historiaDominio.Select(h => new HistoriaResponse()
      {
        ID = h.ID,
        Nome = h.Nome,
        Descricao = h.Descricao,
        ProjetoId = h.ProjetoID,
        NomeProjeto = h.Projeto.Nome
      });
      return Ok(historias);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }



  [HttpGet]
  [Route("ListarHistoriasPorProjeto/{projetoId}")]
  public async Task<ActionResult> ListarHistoriasPorProjeto([FromRoute] int projetoId)
  {
    try
    {
      var historiaDominio = await _historiaApplication.ListarPorProjetoAsync(projetoId);

      var historias = historiaDominio.Select(h => new HistoriaResponse()
      {
        ID = h.ID,
        Nome = h.Nome,
        Descricao = h.Descricao,
        ProjetoId = h.ProjetoID,
        NomeProjeto = h.Projeto.Nome
      });
      return Ok(historias);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }



  [HttpPut]
  [Route("AtualizarHistoria/{historiaId}")]
  public async Task<ActionResult> AtualizarHistoria([FromRoute] int historiaId, [FromBody] HistoriaAtualizar historiaAtualizar)
  {
    try
    {
      var historiaDominio = new Historia()
      {
        ID = historiaId,
        Nome = historiaAtualizar.Nome,
        Descricao = historiaAtualizar.Descricao,
        ProjetoID = historiaAtualizar.ProjetoId
      };

      await _historiaApplication.AtualizarAsync(historiaDominio);

      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }



  [HttpDelete]
  [Route("DeletarHistoria/{historiaId}")]
  public async Task<ActionResult> DeletarHistoria([FromRoute] int historiaId)
  {
    try
    {
      await _historiaApplication.DeletarAsync(historiaId);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

}

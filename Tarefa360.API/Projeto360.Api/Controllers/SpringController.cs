using Microsoft.AspNetCore.Mvc;
using Projeto360.Api.Models.Request;
using Projeto360.Api.Models.Response;
using Projeto360.Application.Interfaces;
using Projeto360.Domain.Entities;

namespace Projeto360.Api;

[ApiController]
[Route("[controller]")]
public class SprintController : ControllerBase
{
  private readonly ISprintApplication _sprintApplication;

  public SprintController(ISprintApplication sprintService)
  {
    _sprintApplication = sprintService;
  }

[HttpPost]
[Route("CriarSprint")]
public async Task<ActionResult> CriarSprint([FromBody] SprintCriar sprintCriar)
{
  try
  {
    var sprintDominio = new Sprint()
    {
      Titulo = sprintCriar.Titulo,
      Descricao = sprintCriar.Descricao,
      ProjetoID = sprintCriar.ProjetoId
    };

    var idSprint = await _sprintApplication.CriarAsync(sprintDominio);

    return Ok(idSprint);
  }
  catch (Exception ex)
  {
    var inner = ex.InnerException?.Message;
    return BadRequest(new { message = ex.Message, inner });
  }
}

  [HttpGet]
  [Route("ObterSprintPorId/{sprintId}")]
  public async Task<ActionResult> ObterSprintPorId([FromRoute] int sprintId)
  {
    try
    {
      var sprintDominio = await _sprintApplication.ObterPorIdAsync(sprintId);

      var sprintResponse = new SprintResponse()
      {
        ID = sprintDominio!.ID,
        Titulo = sprintDominio.Titulo,
        Descricao = sprintDominio.Descricao,
        ProjetoId = sprintDominio.ProjetoID,
        NomeProjeto = sprintDominio.Projeto.Nome
      };

      return Ok(sprintResponse);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpGet]
  [Route("ObterSprintPorTitulo")]
  public async Task<ActionResult> ObterSprintPorTitulo([FromQuery] string titulo)
  {
    try
    {
      var sprintDominio = await _sprintApplication.ObterPorTituloAsync(titulo);

      var sprints = sprintDominio.Select(s => new SprintResponse()
      {
        ID = s.ID,
        Titulo = s.Titulo,
        Descricao = s.Descricao,
        ProjetoId = s.ProjetoID,
        NomeProjeto = s.Projeto.Nome
      });

      return Ok(sprints);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpGet]
  [Route("ListarSprints")]
  public async Task<ActionResult> ListarSprints()
  {
    try
    {
      var sprintDominio = await _sprintApplication.ListarAsync();

      var sprints = sprintDominio.Select(s => new SprintResponse()
      {
        ID = s.ID,
        Titulo = s.Titulo,
        Descricao = s.Descricao,
        ProjetoId = s.ProjetoID,
        NomeProjeto = s.Projeto.Nome
      });

      return Ok(sprints);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpGet]
  [Route("ListarSprintsPorProjeto/{projetoId}")]
  public async Task<ActionResult> ListarSprintsPorProjeto([FromRoute] int projetoId)
  {
    try
    {
      var sprintDominio = await _sprintApplication.ListarPorProjetoAsync(projetoId);

      var sprints = sprintDominio.Select(s => new SprintResponse()
      {
        ID = s.ID,
        Titulo = s.Titulo,
        Descricao = s.Descricao,
        ProjetoId = s.ProjetoID,
        NomeProjeto = s.Projeto.Nome
      });

      return Ok(sprints);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpPut]
  [Route("AtualizarSprint/{sprintId}")]
  public async Task<ActionResult> AtualizarSprint(
      [FromRoute] int sprintId, 
      [FromBody] SprintAtualizar sprintAtualizar)
  {
      try
      {
          var sprintDominio = new Sprint()
          {
              ID = sprintId,
              Titulo = sprintAtualizar.Titulo,
              Descricao = sprintAtualizar.Descricao, 
              ProjetoID = sprintAtualizar.ProjetoId
          };

          await _sprintApplication.AtualizarAsync(sprintDominio);

          return Ok("Sprint atualizada com sucesso");
      }
      catch (Exception)
      {
          return StatusCode(500, "Erro ao atualizar sprint");
      }
  }

  [HttpDelete]
  [Route("DeletarSprint/{sprintId}")]
  public async Task<ActionResult> DeletarSprint([FromRoute] int sprintId)
  {
    try
    {
      await _sprintApplication.DeletarAsync(sprintId);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
}
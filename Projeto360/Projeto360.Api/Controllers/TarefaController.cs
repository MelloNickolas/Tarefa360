using Microsoft.AspNetCore.Mvc;
using Projeto360.Services.Interfaces;
using Projeto360.Application;
using Projeto360.Api.Models.Tarefas.Response;



namespace Projeto360.Api;

[ApiController]
[Route("[controller]")]
public class TarefaController : ControllerBase
{
  private readonly ITarefaAplicacao _tarefaAplicacao;

  public TarefaController(ITarefaAplicacao tarefaAplicacao)
  {
    _tarefaAplicacao = tarefaAplicacao;
  }

  [HttpGet]
  [Route("Obter")]
  public ActionResult Obter()
  {
    try
    {
      var tarefas = _tarefaAplicacao.ListarTarefas();
      var tarefasResposta = tarefas.Select(tarefa => new TarefaResponse
      {
        ID = tarefa.ID,
        Nome = tarefa.Nome,
        Completa = tarefa.Completa
      });

      return Ok(tarefas);
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }
}
using Microsoft.AspNetCore.Mvc;
using Projeto360.Api.Models.Request;
using Projeto360.Api.Models.Response;
using Projeto360.Application.Interfaces;
using Projeto360.Domain.Entities;

namespace Projeto360.Api;

[ApiController]
[Route("[controller]")]
public class EquipeController : ControllerBase
{
  private readonly IEquipeApplication _equipeApplication;

  public EquipeController(IEquipeApplication equipeApplication)
  {
    _equipeApplication = equipeApplication;
  }

  [HttpGet]
  [Route("ObterEquipePorId/{equipeId}")]
  public async Task<ActionResult> ObterEquipePorId([FromRoute] int equipeId)
  {
    try
    {
      var equipeDominio = await _equipeApplication.ObterEquipePorIdAsync(equipeId);

      var equipeResponse = new EquipeResponse()
      {
        ID = equipeDominio.ID,
        Nome = equipeDominio.Nome
      };

      return Ok(equipeResponse);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpGet]
  [Route("ObterEquipePorNome")]
  public async Task<ActionResult> ObterEquipePorNome([FromQuery] string nome)
  {
    try
    {
      var equipeDominio = await _equipeApplication.ObterEquipePorNomeAsync(nome);

      var equipeResponse = equipeDominio.Select(e => new EquipeResponse()
      {
        ID = e.ID,
        Nome = e.Nome
      });

      return Ok(equipeResponse);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpGet]
  [Route("ListarEquipes")]
  public async Task<ActionResult> ListarEquipes()
  {
    try
    {
      var equipeDominio = await _equipeApplication.ListarEquipeAsync();

      var equipes = equipeDominio.Select(equipe => new EquipeResponse()
      {
        ID = equipe.ID,
        Nome = equipe.Nome,
      }).ToList();

      return Ok(equipes);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpGet]
  [Route("ListarMembrosDaEquipe")]
  public async Task<ActionResult> ListarEquipesComMembros()
  {
    try
    {
      var equipes = await _equipeApplication.ListarComMembrosAsync();

      var response = equipes.Select(equipe => new
      {
        id = equipe.ID,
        nome = equipe.Nome,
        membros = equipe.UsuariosEquipe.Select(ue => new
        {
          nomeUsuario = ue.Usuario.Nome,
          papel = ue.PapeisEquipe.ToString()
        }).ToList()
      }).ToList();

      return Ok(response);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }


  [HttpPost]
  [Route("CriarEquipe")]
  public async Task<ActionResult> CriarEquipe([FromBody] EquipeCriar equipeCriar)
  {
    try
    {
      var equipeDominio = new Equipe()
      {
        Nome = equipeCriar.Nome,
      };

      var equipeId = await _equipeApplication.CriarEquipeAsync(equipeDominio);

      return Ok(equipeId);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpPut]
  [Route("AtualizarEquipe/{equipeId}")]
  public async Task<ActionResult> AtualizarEquipe([FromRoute] int equipeId, [FromBody] EquipeAtualizar equipeAtualizar)
  {
    try
    {
      var equipeDominio = new Equipe()
      {
        ID = equipeId,
        Nome = equipeAtualizar.Nome
      };

      await _equipeApplication.AtualizarEquipeAsync(equipeDominio);

      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }


  [HttpDelete]
  [Route("DeletarEquipe/{equipeId}")]
  public async Task<ActionResult> DeletarEquipe([FromRoute] int equipeId)
  {
    try
    {
      await _equipeApplication.DeletarEquipeAsync(equipeId);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

}
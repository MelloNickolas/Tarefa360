using Microsoft.AspNetCore.Mvc;
using Projeto360.Api.Models.Request;
using Projeto360.Api.Models.Response;
using Projeto360.Application.Interfaces;
using Projeto360.Domain.Entities;

namespace Projeto360.Api;

[ApiController]
[Route("[controller]")]
public class UsuarioEquipeController : ControllerBase
{
  private readonly IUsuarioEquipeApplication _usuarioEquipeApplication;
  private readonly IEquipeApplication _equipeApplication;
  private readonly IUsuarioApplication _usuarioApplication;
  public UsuarioEquipeController(
    IUsuarioEquipeApplication usuarioEquipeApplication,
    IEquipeApplication equipeApplication,
    IUsuarioApplication usuarioApplication
    )
  {
    _usuarioEquipeApplication = usuarioEquipeApplication;
    _equipeApplication = equipeApplication;
    _usuarioApplication = usuarioApplication;
  }


  [HttpGet]
  [Route("ObterUsuarioEquipePorId/{usuarioEquipeId}")]
  public async Task<ActionResult> ObterUsuarioEquipePorId([FromRoute] int usuarioEquipeId)
  {
    try
    {
      var usuarioEquipeDominio = await _usuarioEquipeApplication.ObterPorIdAsync(usuarioEquipeId);

      var usuarioEquipeResponse = new UsuarioEquipeResponse()
      {
        ID = usuarioEquipeDominio.ID,
        PapeisEquipe = usuarioEquipeDominio.PapeisEquipe,
        EquipeID = usuarioEquipeDominio.EquipeId,
        NomeEquipe = usuarioEquipeDominio.Equipe.Nome,
        UsuarioID = usuarioEquipeDominio.UsuarioId,
        NomeUsuario = usuarioEquipeDominio.Usuario.Nome
      };

      return Ok(usuarioEquipeResponse);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }


  [HttpGet]
  [Route("ListarUsuarioEquipePorEquipe")]
  public async Task<ActionResult> ListarUsuarioEquipePorEquipe([FromQuery] int equipeID)
  {
    try
    {
      var usuarioEquipeDominio = await _usuarioEquipeApplication.ListarPorEquipesAsync(equipeID);
      if (usuarioEquipeDominio == null)
        return Ok(new List<UsuarioEquipeResponse>());


      var usuarioEquipeLista = usuarioEquipeDominio.Select(relacionamento => new UsuarioEquipeResponse()
      {
        ID = relacionamento.ID,
        PapeisEquipe = relacionamento.PapeisEquipe,
        EquipeID = relacionamento.EquipeId,
        NomeEquipe = relacionamento.Equipe.Nome,
        UsuarioID = relacionamento.UsuarioId,
        NomeUsuario = relacionamento.Usuario.Nome
      }).ToList();

      return Ok(usuarioEquipeLista);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }


  [HttpGet]
  [Route("ListarUsuarioEquipePorUsuario")]
  public async Task<ActionResult> ListarUsuarioEquipePorUsuario([FromQuery] int usuarioID)
  {
    try
    {
      var usuarioEquipeDominio = await _usuarioEquipeApplication.ListarPorUsuariosAsync(usuarioID);
      if (usuarioEquipeDominio == null)
        return Ok(new List<UsuarioEquipeResponse>());


      var usuarioEquipeLista = usuarioEquipeDominio.Select(relacionamento => new UsuarioEquipeResponse()
      {
        ID = relacionamento.ID,
        PapeisEquipe = relacionamento.PapeisEquipe,
        EquipeID = relacionamento.EquipeId,
        NomeEquipe = relacionamento.Equipe.Nome,
        UsuarioID = relacionamento.UsuarioId,
        NomeUsuario = relacionamento.Usuario.Nome
      }).ToList();

      return Ok(usuarioEquipeLista);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpGet]
  [Route("ListarPapeisEquipe")]
  public ActionResult ListarPapeisEquipe()
  {
    try
    {
      var papeisEquipe = _usuarioEquipeApplication.ListarPapeisEquipeAsync();
      var papeisEquipeResponse = papeisEquipe.Select(papel => new PapeisEquipeResponse()
      {
        Id = (int)papel.Key,
        Nome = papel.Value
      }).ToList();
      return Ok(papeisEquipeResponse);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }



  [HttpPost]
  [Route("CriarUsuarioEquipe")]
  public async Task<ActionResult> CriarUsuarioEquipe([FromBody] UsuarioEquipeCriar usuarioEquipeCriar)
  {
    try
    {
      var usuarioEquipeDominio = new UsuarioEquipe()
      {
        PapeisEquipe = usuarioEquipeCriar.PapeisEquipe,
        EquipeId = usuarioEquipeCriar.EquipeID,
        UsuarioId = usuarioEquipeCriar.UsuarioID
      };

      var id = await _usuarioEquipeApplication.CriarAsync(usuarioEquipeDominio);
      return Ok(id);

    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpPut]
  [Route("AtualizarPapelUsuarioEquipePorId/{usuarioEquipeId}")]
  public async Task<ActionResult> AtualizarUsuarioEquipe([FromRoute] int usuarioEquipeId, [FromBody] UsuarioEquipeAtualizar usuarioEquipeAtualizar)
  {
    try
    {
      var usuarioEquipeDominio = new UsuarioEquipe()
      {
        ID = usuarioEquipeId,
        PapeisEquipe = usuarioEquipeAtualizar.PapeisEquipe
      };

      await _usuarioEquipeApplication.AtualizarPapelPorIdAsync(usuarioEquipeDominio);
      return Ok();

    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }


  [HttpDelete]
  [Route("DeletarUsuarioEquipe/{usuarioEquipeId}")]
  public async Task<ActionResult> DeletarUsuarioEquipe([FromRoute] int usuarioEquipeId)
  {
    try
    {
      var usuarioEquipe = new UsuarioEquipe() { ID = usuarioEquipeId };
      await _usuarioEquipeApplication.DeletarAsync(usuarioEquipe);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
}
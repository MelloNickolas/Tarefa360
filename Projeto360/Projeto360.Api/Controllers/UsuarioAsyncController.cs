using Microsoft.AspNetCore.Mvc;
using Projeto360.Dominio.Entidades;
using Projeto360.Api.Models.Request;
using Projeto360.Api.Models.Response;
using Projeto360.Application;

namespace Projeto360.Api;

[ApiController]
[Route("[controller]")]
public class UsuarioAsyncController : ControllerBase
{
  private readonly IUsuarioAplicacao _usuarioAplicao;

  public UsuarioAsyncController(IUsuarioAplicacao usuarioAplicao)
  {
    _usuarioAplicao = usuarioAplicao;
  }

  [HttpPost]
  [Route("CriarAsync")]
  public async Task<ActionResult> Criar([FromBody] UsuarioCriar usuarioCriar)
  {
    try
    {
      var usuarioDominio = new Usuario()
      {
        Nome = usuarioCriar.Nome,
        Email = usuarioCriar.Email,
        Senha = usuarioCriar.Senha
      };

      var IdUsuario = await _usuarioAplicao.CriarAsync(usuarioDominio);

      return Ok(IdUsuario);
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }

  [HttpGet]
  [Route("ObterAsync/{IdUsuario}")]
  public async Task<ActionResult> Obter([FromRoute] int IdUsuario)
  {
    try
    {
      var usuarioDominio = await _usuarioAplicao.ObterAsync(IdUsuario);

      var usuarioResponse = new UsuarioResponse()
      {
        IDUsuario = usuarioDominio.IDUsuario,
        Nome = usuarioDominio.Nome,
        Email = usuarioDominio.Email
      };

      return Ok(usuarioResponse);
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }

  [HttpPut]
  [Route("AtualizarAsync")]
  public async Task<ActionResult> Atualizar([FromBody] UsuarioAtualizar usuarioAtualizar)
  {
    try
    {
      var usuarioDominio = new Usuario()
      {
        IDUsuario = usuarioAtualizar.IDUsuario,
        Nome = usuarioAtualizar.Nome,
        Email = usuarioAtualizar.Email
      };

      await _usuarioAplicao.AtualizarAsync(usuarioDominio);

      return Ok(usuarioDominio);
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }

  [HttpPut]
  [Route("AtualizarSenhaAsync")]
  public async Task<ActionResult> AtualizarSenha([FromBody] UsuarioAtualizarSenha usuarioAtualizarSenha)
  {
    try
    {
      var usuarioDominio = new Usuario()
      {
        IDUsuario = usuarioAtualizarSenha.IDUsuario,
        Senha = usuarioAtualizarSenha.Senha
      };

      await _usuarioAplicao.AtualizarSenhaAsync(usuarioDominio, usuarioAtualizarSenha.SenhaAntiga);

      return Ok(usuarioDominio);
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }

  [HttpDelete]
  [Route("DeletarAsync/{IdUsuario}")]
  public async Task<ActionResult> Deletar([FromRoute] int IdUsuario)
  {
    try
    {
      await _usuarioAplicao.DeletarAsync(IdUsuario);
      return Ok();
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }

  [HttpPut]
  [Route("RestaurarAsync/{IdUsuario}")]
  public async Task<ActionResult> Restaurar([FromRoute] int IdUsuario)
  {
    try
    {
      await _usuarioAplicao.RestaurarAsync(IdUsuario);
      return Ok();
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }

  [HttpGet]
  [Route("ListarAsync")]
  public async Task<ActionResult> Listar([FromQuery] bool ativos)
  {
    try
    {
      var usuarioDominio = await _usuarioAplicao.ListarAsync(ativos);

      var usuarios = usuarioDominio.Select(x => new UsuarioResponse()
      {
        IDUsuario = x.IDUsuario,
        Nome = x.Nome,
        Email = x.Email
      }).ToList();

      return Ok(usuarios);
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }
}

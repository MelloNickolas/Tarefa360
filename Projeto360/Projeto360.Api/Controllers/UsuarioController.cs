using Microsoft.AspNetCore.Mvc;
using Projeto360.Dominio.Entidades;
using Projeto360.Api.Models.Request;
using Projeto360.Api.Models.Response;
using Projeto360.Application;



namespace Projeto360.Api;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
  private readonly IUsuarioAplicacao _usuarioAplicao;

  public UsuarioController(IUsuarioAplicacao usuarioAplicao)
  {
    _usuarioAplicao = usuarioAplicao;
  }

  [HttpPost]
  [Route("Criar")]
  public ActionResult Criar([FromBody] UsuarioCriar usuarioCriar)
  {
    try
    {
      var usuarioDominio = new Usuario()
      {
        Nome = usuarioCriar.Nome,
        Email = usuarioCriar.Email,
        Senha = usuarioCriar.Senha,
        TipoUsuario = usuarioCriar.TipoUsuario
      };

      var IdUsuario = _usuarioAplicao.Criar(usuarioDominio);

      return Ok(IdUsuario);
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }

  [HttpGet]
  [Route("Obter/{IdUsuario}")]
  public ActionResult Obter([FromRoute] int IdUsuario)
  {
    try
    {
      var usuarioDominio = _usuarioAplicao.Obter(IdUsuario);

      var usuarioResponse = new UsuarioResponse()
      {
        IDUsuario = usuarioDominio.IDUsuario,
        Nome = usuarioDominio.Nome,
        Email = usuarioDominio.Email,
        TipoUsuario = usuarioDominio.TipoUsuario
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
  [Route("Atualizar")]
  public ActionResult Atualizar([FromBody] UsuarioAtualizar usuarioAtualizar)
  {
    try
    {
      var usuarioDominio = new Usuario()
      {
        IDUsuario = usuarioAtualizar.IDUsuario,
        Nome = usuarioAtualizar.Nome,
        Email = usuarioAtualizar.Email,
        TipoUsuario = usuarioAtualizar.TipoUsuario
      };

      _usuarioAplicao.Atualizar(usuarioDominio);

      return Ok(usuarioDominio);
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }

  [HttpPut]
  [Route("AtualizarSenha")]
  public ActionResult AtualizarSenha([FromBody] UsuarioAtualizarSenha usuarioAtualizarSenha)
  {
    try
    {
      var usuarioDominio = new Usuario()
      {
        IDUsuario = usuarioAtualizarSenha.IDUsuario,
        Senha = usuarioAtualizarSenha.Senha
      };

      _usuarioAplicao.AtualizarSenha(usuarioDominio, usuarioAtualizarSenha.SenhaAntiga);

      return Ok(usuarioDominio);
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }

  [HttpDelete]
  [Route("Deletar/{IdUsuario}")]
  public ActionResult Deletar([FromRoute] int IdUsuario)
  {
    try
    {
      _usuarioAplicao.Deletar(IdUsuario);
      return Ok();
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }

  [HttpPut]
  [Route("Restaurar/{IdUsuario}")]
  public ActionResult Restaurar([FromRoute] int IdUsuario)
  {
    try
    {
      _usuarioAplicao.Restaurar(IdUsuario);
      return Ok();
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }

  [HttpGet]
  [Route("Listar")]
  public ActionResult Listar([FromQuery] bool ativos)
  {
    try
    {
      var usuarioDominio = _usuarioAplicao.Listar(ativos);

      var usuarios = usuarioDominio.Select(x => new UsuarioResponse()
      {
        IDUsuario = x.IDUsuario,
        Nome = x.Nome,
        Email = x.Email,
        TipoUsuario = x.TipoUsuario
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
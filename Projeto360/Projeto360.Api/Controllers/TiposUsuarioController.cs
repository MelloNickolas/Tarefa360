using Microsoft.AspNetCore.Mvc;
using Projeto360.Dominio.Entidades;
using Projeto360.Dominio.Enumeradores;
using Projeto360.Api.Models.Request;
using Projeto360.Api.Models.Response;
using Projeto360.Application;

namespace Projeto360.Api;

[ApiController]
[Route("api/[controller]")]
public class EnumController : ControllerBase
{
  [HttpGet("ListarTiposUsuario")]
  public IActionResult ListarTiposUsuario()
  {
    try
    {
      var lista = Enum.GetValues(typeof(TipoUsuario))
          .Cast<TipoUsuario>() //Comprova que cada objeto é um TipoUsuario
          .Select(t => new TipoUsuarioResponse
          {
            Id = (int)t,
            Nome = t.ToString()
          })
          .ToList();

      return Ok(lista);
    }
    catch (Exception ex)
    {
      var inner = ex.InnerException?.Message;
      return BadRequest(new { message = ex.Message, inner });
    }
  }
}
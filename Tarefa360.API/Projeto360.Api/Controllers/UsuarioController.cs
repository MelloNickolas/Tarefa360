using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Projeto360.Api.Models.Request;
using Projeto360.Api.Models.Response;
using Projeto360.Application.Interfaces;
using Projeto360.Domain.Entities;
using Projeto360.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Projeto360.Api;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioApplication _usuarioApplication;

     private readonly IConfiguration _config;

    public UsuarioController(IUsuarioApplication usuarioAplication, IConfiguration config)
    {
        _usuarioApplication = usuarioAplication;
        _config = config;
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet]
    [Route("ObterPorId/{usuarioId}")]
    public async Task<ActionResult> ObterPorId([FromRoute] int usuarioId)
    {
        try
        {
            var usuarioDominio = await _usuarioApplication.ObterPorIdAsync(usuarioId);

            var usuarioResponse = new UsuarioResponse()
            {
                Id = usuarioDominio.ID,
                Nome = usuarioDominio.Nome,
                Email = usuarioDominio.Email,
                TipoUsuarioId = (int)usuarioDominio.TipoUsuario
            };

            return Ok(usuarioResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet]
    [Route("ObterPorEmail")]
    public async Task<ActionResult> ObterPorEmail([FromQuery] string email)
    {
        try
        {
            var usuarioDominio = await _usuarioApplication.ObterPorEmailAsync(email);

            var usuarioResponse = new UsuarioResponse()
            {
                Id = usuarioDominio.ID,
                Nome = usuarioDominio.Nome,
                Email = usuarioDominio.Email,
                TipoUsuarioId = (int)usuarioDominio.TipoUsuario
            };

            return Ok(usuarioResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet]
    [Route("Listar")]
    public async Task<ActionResult> Listar([FromQuery] bool ativo)
    {
        try
        {
            var usuariosDominio = await _usuarioApplication.ListarAsync(ativo);
            var usuarios = usuariosDominio.Select(usuario => new UsuarioResponse()
            {
                Id = usuario.ID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                TipoUsuarioId = (int)usuario.TipoUsuario
            }).ToList();

            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet]
    [Route("ListarTiposUsuario")]
    public  ActionResult ListarTiposUsuario()
    {
        try
        {
            var tiposUsuario = _usuarioApplication.ListarTiposUsuarioAsync();
            var tiposUsuarioResponse = tiposUsuario.Select(tipo => new TiposUsuarioResponse()
            {
                Id = (int)tipo.Key,
                Nome = tipo.Value
            }).ToList();
            return Ok(tiposUsuarioResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [Route("Criar")]
    public async Task<ActionResult> Criar([FromBody] UsuarioCriar usuario)
    {
        try
        {
            var usuarioDominio = new Usuario()
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                TipoUsuario = (TiposUsuario)usuario.TipoUsuarioId
            };

            var usuarioId = await _usuarioApplication.CriarAsync(usuarioDominio);

            return Ok(usuarioId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut]
    [Route("Atualizar/{usuarioId}")]
    public async Task<ActionResult> Atualizar([FromRoute] int usuarioId, [FromBody] UsuarioAtualizar usuario)
    {
        try
        {
            var usuarioDominio = new Usuario()
            {
                ID = usuarioId,
                Nome = usuario.Nome,
                Email = usuario.Email,
                TipoUsuario = (TiposUsuario)usuario.TipoUsuarioId
            };

            await _usuarioApplication.AtualizarAsync(usuarioDominio);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut]
    [Route("AlterarSenha")]
    public async Task<ActionResult> AlterarSenha([FromBody] UsuarioAlterarSenha usuarioAlterarSenha)
    {
        try
        {
            var usuarioDominio = new Usuario()
            {
                ID = usuarioAlterarSenha.ID,
                Senha = usuarioAlterarSenha.novaSenha
            };

            await _usuarioApplication.AtualizarSenhaAsync(usuarioDominio, usuarioAlterarSenha.senhaAtual);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete]
    [Route("Deletar/{usuarioId}")]
    public async Task<ActionResult> Deletar([FromRoute] int usuarioId)
    {
        try
        {
            await _usuarioApplication.DeletarAsync(usuarioId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut]
    [Route("Restaurar/{usuarioId}")]
    public async Task<ActionResult> Restaurar([FromRoute] int usuarioId)
    {
        try
        {
            await _usuarioApplication.RestaurarAsync(usuarioId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]  
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginRequest login)
    {
        var usuariologin = await _usuarioApplication.ObterPorEmailESenhaAsync(login.Email, login.Senha);

        if (usuariologin == null)
            return Unauthorized("Usuário ou senha inválidos");

        var token = GerarToken(usuariologin);

        return Ok(new { token });
    }

    private string GerarToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("Id", usuario.ID.ToString()),
                new Claim(ClaimTypes.Role, usuario.TipoUsuario.ToString())
            }),

            Expires = DateTime.UtcNow.AddHours(2),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
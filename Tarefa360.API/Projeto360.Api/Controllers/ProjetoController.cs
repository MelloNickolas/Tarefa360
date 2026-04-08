using Microsoft.AspNetCore.Mvc;
using Projeto360.Api.Models.Request;
using Projeto360.Api.Models.Response;
using Projeto360.Application.Interfaces;
using Projeto360.Domain.Entities;

namespace Projeto360.Api;

[ApiController]
[Route("[controller]")]
public class ProjetoController : ControllerBase
{
    private readonly IProjetoApplication _projetoApplication;

    public ProjetoController(IProjetoApplication projetoAplication)
    {
        _projetoApplication = projetoAplication;
    }

    [HttpGet]
    [Route("ObterProjetoPorId/{projetoId}")]
    public async Task<ActionResult> ObterProjetoPorId([FromRoute] int projetoId)
    {
        try
        {
            var projetoDominio = await _projetoApplication.ObterProjetoPorIdAsync(projetoId);

            var projetoResponse = new ProjetoResponse()
            {
                Id = projetoDominio.ID,
                Nome = projetoDominio.Nome,
                Descricao = projetoDominio.Descricao,
            };

            return Ok(projetoResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("ObterProjetoPorNome")]
    public async Task<ActionResult> ObterProjetoPorNome([FromQuery] string nome)
    {
        try
        {
            var projetoDominio = await _projetoApplication.ObterProjetoPorNomeAsync(nome);

            var projetoResponse = new ProjetoResponse()
            {
                Id = projetoDominio.ID,
                Nome = projetoDominio.Nome,
                Descricao = projetoDominio.Descricao,
            };

            return Ok(projetoResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("ListarProjeto")]
    public async Task<ActionResult> ListarProjeto([FromQuery] bool ativo)
    {
        try
        {
            var projetosDominio = await _projetoApplication.ListarProjetoAsync(ativo);
            var projetos = projetosDominio.Select(projeto => new ProjetoResponse()
            {
                Id = projeto.ID,
                Nome = projeto.Nome,
                Descricao = projeto.Descricao,
            }).ToList();

            return Ok(projetos);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    /* Criando um ENDPOINT para passar somente os dados necessários para o dropdown*/
    [HttpGet]
    [Route("ListarProjetoDropdown")]
    public async Task<ActionResult> ListarProjetoDropdown([FromQuery] bool ativo)
    {
        try
        {
            var projetosDominio = await _projetoApplication.ListarProjetoAsync(ativo);
            var projetos = projetosDominio.Select(projeto => new ListarProjetosResponse()
            {
                ID = projeto.ID,
                Nome = projeto.Nome,
            }).ToList();

            return Ok(projetos);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }





    [HttpPost]
    [Route("CriarProjeto")]
    public async Task<ActionResult> CriarProjeto([FromBody] ProjetoCriar projeto)
    {
        try
        {
            var projetoDominio = new Projeto()
            {
                Nome = projeto.Nome,
                Descricao = projeto.Descricao,
            };

            var projetoId = await _projetoApplication.CriarProjetoAsync(projetoDominio);

            return Ok(projetoId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Route("AtualizarProjeto/{projetoId}")]
    public async Task<ActionResult> AtualizarProjeto([FromRoute] int projetoId, [FromBody] ProjetoAtualizar projeto)
    {
        try
        {
            var projetoDominio = new Projeto()
            {
                ID = projetoId,
                Nome = projeto.Nome,
                Descricao = projeto.Descricao,
            };

            await _projetoApplication.AtualizarProjetoAsync(projetoDominio);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete]
    [Route("DeletarProjeto/{projetoId}")]
    public async Task<ActionResult> DeletarProjeto([FromRoute] int projetoId)
    {
        try
        {
            await _projetoApplication.DeletarProjetoAsync(projetoId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Route("RestaurarProjeto/{projetoId}")]
    public async Task<ActionResult> RestaurarProjeto([FromRoute] int projetoId)
    {
        try
        {
            await _projetoApplication.RestaurarProjetoAsync(projetoId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
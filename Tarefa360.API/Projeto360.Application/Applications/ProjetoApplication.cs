using Projeto360.Domain.Entities;
using Projeto360.Repository.Interfaces;
using Projeto360.Application.Interfaces;

namespace Projeto360.Application;

public class ProjetoApplication : IProjetoApplication
{
    private readonly IProjetoRepository _projetoRepository;

    public ProjetoApplication(IProjetoRepository projetoRepository)
    {
        _projetoRepository = projetoRepository;
    }

    public async Task<int> CriarProjetoAsync(Projeto projeto)
    {
        if (projeto == null)
            throw new Exception("Projeto não pode ser vazio.");
        ValidarInformacoesProjeto(projeto);

        var projetoExistente = await _projetoRepository.ObterProjetoPorNomeAsync(projeto.Nome);
        if (projetoExistente != null)
            throw new Exception("Já existe um projeto com o nome informado.");

        if (string.IsNullOrWhiteSpace(projeto.Descricao))
            throw new Exception("Descrição não pode ser vazia.");
        
        return await _projetoRepository.CriarProjetoAsync(projeto);
    }

    public async Task AtualizarProjetoAsync(Projeto projeto)
    {
        Projeto projetoExistenteId = await ValidarProjetoExistentePorId(projeto.ID);

        var projetoExistenteNome = await _projetoRepository.ObterProjetoPorNomeAsync(projeto.Nome);
        if (projetoExistenteNome != null && projetoExistenteNome.ID != projetoExistenteId.ID)
            throw new Exception("Já existe projeto com o nome informado.");

        ValidarInformacoesProjeto(projeto);

        projetoExistenteId.Nome = projeto.Nome;
        projetoExistenteId.Descricao = projeto.Descricao;

        await _projetoRepository.AtualizarProjetoAsync(projetoExistenteId);
    }

    public async Task<Projeto> ObterProjetoPorIdAsync(int projetoID)
    {
        Projeto projetoExistente = await ValidarProjetoExistentePorId(projetoID);

        return projetoExistente;
    }

    public async Task<Projeto> ObterProjetoPorNomeAsync(string nome)
    {
        var projetoExistente = await _projetoRepository.ObterProjetoPorNomeAsync(nome);
        if (projetoExistente == null)
            throw new Exception("Projeto não localizado.");

        return projetoExistente;
    }

    public async Task DeletarProjetoAsync(int projetoID)
    {
        Projeto projetoExistente = await ValidarProjetoExistentePorId(projetoID);

        projetoExistente.Deletar();

        await _projetoRepository.AtualizarProjetoAsync(projetoExistente);
    }

    public async Task RestaurarProjetoAsync(int projetoId)
    {
        Projeto projetoExistente = await ValidarProjetoExistentePorId(projetoId);

        projetoExistente.Restaurar();

        await _projetoRepository.AtualizarProjetoAsync(projetoExistente);
    }

    public async Task<IEnumerable<Projeto>> ListarProjetoAsync(bool ativo)
    {
        return await _projetoRepository.ListarProjetoAsync(ativo);
    }

    #region Úteis
    private static void ValidarInformacoesProjeto(Projeto projeto)
    {
        if (string.IsNullOrWhiteSpace(projeto.Nome))
            throw new Exception("Nome do projeto não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(projeto.Descricao))
            throw new Exception("Descrição do projeto não pode ser vazia.");
    }

    private async Task<Projeto> ValidarProjetoExistentePorId(int projetoId)
    {
        var projetoExistente = await _projetoRepository.ObterProjetoPorIdAsync(projetoId);
        if (projetoExistente == null)
            throw new Exception("Projeto não localizado.");
        return projetoExistente;
    }

    #endregion
}
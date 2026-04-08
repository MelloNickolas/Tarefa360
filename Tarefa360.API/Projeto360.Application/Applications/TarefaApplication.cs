using Projeto360.Domain.Entities;
using Projeto360.Application.Interfaces;
using Projeto360.Domain.Enums;
using Projeto360.Repository.Interfaces;

namespace Projeto360.Application;

public class TarefaApplication : ITarefaApplication
{
    private readonly ITarefaRepository _tarefaRepository;
    private readonly IProjetoApplication _projetoApplication;
    private readonly IHistoriaApplication _historiaApplication;
    private readonly IUsuarioApplication _usuarioApplication;
    private readonly ISprintApplication _sprintApplication;

    public TarefaApplication(
        ITarefaRepository tarefaRepository,
        IProjetoApplication projetoApplication,
        IHistoriaApplication historiaApplication,
        IUsuarioApplication usuarioApplication,
        ISprintApplication sprintApplication
        )
    {
        _tarefaRepository = tarefaRepository;
        _projetoApplication = projetoApplication;
        _historiaApplication = historiaApplication;
        _usuarioApplication = usuarioApplication;
        _sprintApplication = sprintApplication;
    }

    public async Task<int> CriarAsync(Tarefa tarefaDTO)
    {
        var (projeto, historia, usuario, sprint) = await ValidacaoInformacoesTarefa(tarefaDTO);

        return await _tarefaRepository.CriarAsync(tarefaDTO);
    }

    public async Task AtualizarAsync(Tarefa tarefaDTO)
    {
        var tarefaExistente = await ValidarTarefaExistentePorId(tarefaDTO.ID);

        var (projeto, historia, usuario, sprint) = await ValidacaoInformacoesTarefa(tarefaDTO);

        tarefaExistente.Titulo = tarefaDTO.Titulo;
        tarefaExistente.Descricao = tarefaDTO.Descricao;
        tarefaExistente.Estimativa = tarefaDTO.Estimativa;
        tarefaExistente.TipoTarefa = tarefaDTO.TipoTarefa;
        tarefaExistente.ProjetoID = projeto.ID;
        tarefaExistente.HistoriaID = historia.ID;
        tarefaExistente.UsuarioID = usuario.ID;
        tarefaExistente.SprintID = sprint.ID;

        await _tarefaRepository.AtualizarAsync(tarefaExistente);
    }

    public async Task DeletarAsync(int tarefaID)
    {
        var tarefaExistente = await ValidarTarefaExistentePorId(tarefaID);

        await _tarefaRepository.DeletarAsync(tarefaExistente);
    }

    public async Task ConcluirTarefaAsync(int tarefaID)
    {
        var tarefaExistente = await ValidarTarefaExistentePorId(tarefaID);
        tarefaExistente.ConcluirTarefa();

        await _tarefaRepository.AtualizarAsync(tarefaExistente);
    }

    public async Task RetomarTarefaAsync(int tarefaID)
    {
        var tarefaExistente = await ValidarTarefaExistentePorId(tarefaID);
        tarefaExistente.RetomarTarefa();

        await _tarefaRepository.AtualizarAsync(tarefaExistente);
    }

    public async Task<IEnumerable<Tarefa>> ListarAsync(bool concluida)
    {
        return await _tarefaRepository.ListarAsync(concluida);
    }

    public async Task<IEnumerable<Tarefa>> ListarTodasAsync()
    {
        return await _tarefaRepository.ListarTodasAsync();
    }

    public Task<Dictionary<TiposTarefa, string>> ListarTiposTarefaAsync()
    {
        // Cria um dicionário para armazenar os tipos de tarefa e suas descrições
        var tiposTarefa = new Dictionary<TiposTarefa, string>();

        // Percorre os valores do enum TiposTarefa e adiciona ao dicionário
        foreach (TiposTarefa tipo in Enum.GetValues(typeof(TiposTarefa)))
        {
            tiposTarefa[tipo] = tipo.ToString();
        }

        return Task.FromResult(tiposTarefa);
    }

    public async Task<Tarefa> ObterPorIdAsync(int tarefaID)
    {
        var tarefaExistente = await ValidarTarefaExistentePorId(tarefaID);

        return tarefaExistente;
    }

    #region Úteis
    private async Task<(Projeto projeto, Historia historia, Usuario usuario, Sprint sprint)> ValidacaoInformacoesTarefa(Tarefa tarefaDTO)
    {
        // Valida se a tarefa nao esta vazia
        if (tarefaDTO == null)
            throw new Exception("Tarefa nao pode ser vazia.");

        // Valida se o título não está vazio ou foi enviado apenas espaços
        if (string.IsNullOrWhiteSpace(tarefaDTO.Titulo))
            throw new Exception("Titulo nao pode ser vazio.");

        // Valida se a descrição não está vazio ou foi enviado apenas espaços
        if (string.IsNullOrWhiteSpace(tarefaDTO.Descricao))
            throw new Exception("Descricao nao pode ser vazia.");

        // Se a estimativa tiver sido preenchida, valida se ela não é negativa
        if (tarefaDTO.Estimativa.HasValue && tarefaDTO.Estimativa < 0)
            throw new Exception("Estimativa nao pode ser negativa.");

        // Valida se o tipo de tarefa foi preenchido e é um tipo valido do enum
        if (!Enum.IsDefined(typeof(TiposTarefa), tarefaDTO.TipoTarefa))
            throw new Exception("Tipo de tarefa inválido.");
        if (tarefaDTO.ProjetoID <= 0 || tarefaDTO.HistoriaID <= 0 || tarefaDTO.UsuarioID <= 0)
            throw new Exception("ID Invalido.");

        var projeto = await ObterProjetoPorId(tarefaDTO.ProjetoID);
        var historia = await ObterHistoriaPorId(tarefaDTO.HistoriaID);
        var usuario = await ObterUsuarioPorId(tarefaDTO.UsuarioID);
        var sprint = await ObterSprintPorId(tarefaDTO.SprintID);

        if (historia.ProjetoID != projeto.ID)
            throw new Exception("História não pertence ao projeto informado.");

        return (projeto, historia, usuario, sprint);
    }

    private async Task<Tarefa> ValidarTarefaExistentePorId(int tarefaID)
    {
        var tarefaExistente = await _tarefaRepository.ObterPorIdAsync(tarefaID);
        if (tarefaExistente == null)
            throw new Exception("Tarefa nao localizada.");

        return tarefaExistente;
    }

    // No application já tem a valiação se existe com esse ID, 
    // então aqui só precisa fazer o get
    // Se não existir vai estourar Exception
    private async Task<Projeto> ObterProjetoPorId(int projetoId)
        => await _projetoApplication.ObterProjetoPorIdAsync(projetoId);

    private async Task<Historia> ObterHistoriaPorId(int historiaId)
        => await _historiaApplication.ObterPorIdAsync(historiaId);

    private async Task<Usuario> ObterUsuarioPorId(int usuarioId)
        => await _usuarioApplication.ObterPorIdAsync(usuarioId);

    private async Task<Sprint> ObterSprintPorId(int sprintId)
        => await _sprintApplication.ObterPorIdAsync(sprintId);

    #endregion
}

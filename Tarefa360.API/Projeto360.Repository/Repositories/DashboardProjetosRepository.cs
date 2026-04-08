using Dapper;
using Microsoft.EntityFrameworkCore;
using Projeto360.Repository.Interfaces;
using Projeto360.Repository.DTOs;
using Projeto360.Domain.Enums;
using Projeto360.Domain.Entities;

namespace Projeto360.Repository;

public class DashboardProjetosRepository : BaseRepository, IDashboardProjetosRepository
{
    public DashboardProjetosRepository(Projeto360Context context) : base(context) { }

    // Quantidade total de histórias em todos os projetos
    public async Task<int> QtdTotalHistoriasAsync()
    {
        const string sql = @"SELECT COUNT(ID) FROM Historias";

        var connection = _context.Database.GetDbConnection();

        var total = await connection.ExecuteScalarAsync<int>(sql);

        return total;
    }

    // Quantidade de histórias vinculadas ao projeto (filtro por ProjetoID)
    public async Task<int> QtdHistoriasPorProjetoAsync(int ProjetoID)
    {
        const string sql = @"
        SELECT COUNT(h.ID) AS Qtd_Historias
        FROM Historias h
        INNER JOIN Projetos p ON h.ProjetoID = p.ID
        WHERE h.ProjetoID = @ProjetoID";

        var connection = _context.Database.GetDbConnection();

        var total = await connection.QueryFirstOrDefaultAsync<int>(sql, new { ProjetoID = ProjetoID });

        return total;
    }

    // Quantidade de histórias vinculadas ao projeto (agrupado por projeto)
    public async Task<IEnumerable<HistoriasPorProjetoDTO>> QtdHistoriasPorProjetoAgrupadoAsync()
    {
        const string sql = @"
            SELECT
                p.ID AS ProjetoID,
                p.Nome AS Projeto, 
                COUNT(h.ID) AS Qtd_Historias
            FROM Historias h
            INNER JOIN Projetos p ON h.ProjetoID = p.ID
            GROUP BY p.Nome
            ORDER BY Qtd_Historias DESC";

        var connection = _context.Database.GetDbConnection();

        var total = await connection.QueryAsync<HistoriasPorProjetoDTO>(sql);

        return total;
    }

    // Quantidade de histórias concluídas ou pendentes
    public async Task<int> QtdHistoriasPorConclusaoAsync(bool Concluido)
    {
        const string sql = @"
        SELECT
            COUNT(t.ID) AS Qtd_tarefas
        FROM Tarefas t
        WHERE Concluido = @Concluido
        GROUP BY t.Concluido";

        var connection = _context.Database.GetDbConnection();

        var total = await connection.QueryAsync<int>(sql, new { Concluido = Concluido });

        return total.FirstOrDefault();
    }

    // Total de tarefas por Tipo
    public async Task<IEnumerable<TarefasPorTipoDTO>> QtdTotalTarefasAgrupadoPorTipoAsync()
    {
        const string sql = @"
        SELECT 
            t.TipoTarefa,
            COUNT(t.ID) AS Qtd_tarefas
        FROM Tarefas t
        GROUP BY t.TipoTarefa";

        var connection = _context.Database.GetDbConnection();

        var total = await connection.QueryAsync<TarefasPorTipoDTO>(sql);

        return total;
    }

    // Quantidade de tarefas concluídas ou em aberto agrupado por Tipo
    public async Task<IEnumerable<TarefasPorTipoDTO>> QtdTarefasAgrupadoPorTipoPorConclusaoAsync(bool Concluido)
    {
        const string sql = @"
        SELECT 
            t.TipoTarefa,
            COUNT(t.ID) AS Qtd_tarefas
        FROM Tarefas t
        WHERE Concluido = @Concluido
        GROUP BY t.TipoTarefa, t.Concluido";

        var connection = _context.Database.GetDbConnection();

        var total = await connection.QueryAsync<TarefasPorTipoDTO>(sql, new { Concluido = Concluido });

        return total;
    }

    // Quantidade de tarefas concluídas ou em aberto agrupado por Tipo
    public async Task<int> QtdTotalTarefasPorTipoPorConclusaoAsync(TiposTarefa tipoTarefa, bool Concluido)
    {
        const string sql = @"
        SELECT 
            COUNT(t.ID) AS Qtd_tarefas
        FROM Tarefas t
        WHERE Concluido = @Concluido AND t.TipoTarefa = @tipoTarefa";

        var connection = _context.Database.GetDbConnection();

        var total = await connection.QueryAsync<int>(sql, new { Concluido = Concluido, tipoTarefa = tipoTarefa });

        return total.FirstOrDefault();
    }

    // Quantidade total de tarefas concluídas ou em aberto
    public async Task<int> QtdTotalTarefasPorConclusaoAsync(bool Concluido)
    {
        const string sql = @"
        SELECT
            COUNT(ID) AS Qtd_tarefas
        FROM Tarefas
        WHERE Concluido = @Concluido";

        var connection = _context.Database.GetDbConnection();

        var total = await connection.QueryAsync<int>(sql, new { Concluido = Concluido });

        return total.FirstOrDefault();
    }

    // Quantidade de horas concluidas ou pendentes de acordo com a estimativa da tarefa
    public async Task<int> QtdHorasPorConclusaoAsync(bool Concluido)
    {
        const string sql = @"
        SELECT
            SUM(Estimativa) AS Total_Horas
        FROM Tarefas
        WHERE Concluido = @Concluido";

        var connection = _context.Database.GetDbConnection();

        var total = await connection.QueryAsync<int>(sql, new { Concluido = Concluido });

        return total.FirstOrDefault();
    }

    public async Task<IEnumerable<Tarefa>> ListarTarefasConcluidasDataAtualAsync()
    {
        const string sql = @"
        SELECT 
            t.*,
            u.ID AS UsuarioID, 
            u.Nome, 
            u.Email, 
            u.TipoUsuario
        FROM Tarefas t
        INNER JOIN Usuarios u ON t.UsuarioID = u.ID
        WHERE t.Concluido = 1 
        AND DATE(t.DataConclusao) = DATE('now')
        ORDER BY t.DataConclusao DESC;";

        var connection = _context.Database.GetDbConnection();

        var tarefas = await connection.QueryAsync<Tarefa, Usuario, Tarefa>(
            sql,
            (tarefa, usuario) =>
            {
                tarefa.Usuario = usuario;
                return tarefa;
            },
            splitOn: "UsuarioID"
        );

        return tarefas;
    }
}
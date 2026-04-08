using Projeto360.Domain.Enums;

namespace Projeto360.Domain.Entities;

public class Tarefa
{
    public int ID { get; set; }
    public string Titulo { get; set; } //required
    public string Descricao { get; set; } //required
    public decimal? Estimativa { get; set; } 
    public TiposTarefa TipoTarefa { get; set; } //required
    public bool Concluido { get; private set; } //required
    public DateTime DataCriacao { get; set; } //required
    public DateTime? DataConclusao { get; set; }

    #region Relacionamentos

    // Campo para relacionamento, Vamos puxar o ID para relacionar o Projeto
    public int ProjetoID { get; set; } //required
    public Projeto Projeto { get; set; }

    // Campo para relacionamento, Vamos puxar o ID para relacionara Historia
    public int HistoriaID { get; set; } //required
    public Historia Historia { get; set; }
    /* Esse campo serve para podermos navegar nas propriedades da historia relacionada */


    // Campo para relacionamento, Vamos puxar o ID para relacionar a Sprint
    public int SprintID { get; set; } //required
    public Sprint Sprint { get; set; }
    /* Esse campo serve para podermos navegar nas propriedades da sprint relacionada */


    // Campo para relacionamento, Vamos puxar o ID para relacionar o Usuario
    public int UsuarioID { get; set; } //required
    public Usuario Usuario { get; set; }
    /* Esse campo serve para podermos navegar nas propriedades do Usuario relacionada */

    #endregion

    public Tarefa()
    {
        Concluido = false;
        DataCriacao = DateTime.Now;
    }

    public void ConcluirTarefa()
    {
        Concluido = true;
        DataConclusao = DateTime.Now;
    }

    public void RetomarTarefa()
    {
        Concluido = false;
        DataConclusao = null;
    }
}
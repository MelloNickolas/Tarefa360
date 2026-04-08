namespace Projeto360.Domain.Entities;

public class Projeto
{
    public int ID { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }   
    public bool Ativo { get; set; }


    // Campo de relacionamento muitos para Historia e para Sprint
    // Collection é um List mais flexível da uma pesquisada sobre!
    public ICollection<Historia> Historias { get; set; }
    public ICollection<Sprint> Sprints { get; set; }
    public ICollection<Tarefa> Tarefas { get; set; }

    public Projeto()
    {
        Ativo = true;
    }

    public void Deletar()
    {
        Ativo = false;
    }

    public void Restaurar()
    {
        Ativo = true;
    }
}

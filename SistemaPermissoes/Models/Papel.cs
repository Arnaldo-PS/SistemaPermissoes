using System.ComponentModel.DataAnnotations;

public class Papel
{
    [Key]
    public int Handle { get; set; }
    public string Nome { get; set; }
    public List<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}

public class Papel
{
    public int Id { get; set; }
    public string Nome { get; set; }

    // Lista de tarefas associadas a esse papel
    public List<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}

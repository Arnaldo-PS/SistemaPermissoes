using System.ComponentModel.DataAnnotations;


public class RoleCreateViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public List<TarefaViewModel> Tarefas { get; set; } 

}

public class TarefaViewModel
{
    public int TarefaId { get; set; }
    public string TarefaNome { get; set; }
    public bool IsAssigned { get; set; }
}


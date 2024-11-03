using System.ComponentModel.DataAnnotations;

public class Tarefa
{
    [Key]
    public int Handle { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
using System.ComponentModel.DataAnnotations;

public class Usuario
{
    [Key]
    public int Handle { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public List<Papel> Papeis { get; set; }
}

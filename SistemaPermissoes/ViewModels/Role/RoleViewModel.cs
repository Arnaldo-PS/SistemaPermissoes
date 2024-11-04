using System.ComponentModel.DataAnnotations;


public class RoleViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do papel é obrigatório.")]
    [StringLength(30, ErrorMessage = "O nome do papel não pode exceder 30 caracteres.")]
    public string Nome { get; set; }
    public List<string> Tarefas { get; set; }

}

